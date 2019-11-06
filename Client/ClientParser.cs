﻿using MSDAD.Client.Exceptions;
using MSDAD.Client.Commands.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MSDAD.Client.Commands;
using MSDAD.Client.Commands.Parser;

namespace MSDAD.Client
{
    class ClientParser
    {
        const string PING_COMMAND = "ping";
        const string CREATE = "create";
        const string EXIT = "exit";
        const string LIST = "list";
        const string JOIN = "join";
        const string CLOSE = "close";

        string script_name;

        ClientLibrary clientLibrary;
        Command command;

        public ClientParser(string script_name)
        {
            // TODO portos pre definidos que na verdade sao escolhidos pelo puppet master
            this.clientLibrary = new ClientLibrary(ClientUtils.GetLocalIPAddress(), 1488);
            new Initialize(ref this.clientLibrary);

            this.script_name = script_name;
        }
        public void Parse()
        {            
            string script_path;

            script_path = this.AssembleScript();            

            if (this.ScriptExists(script_path))
            {
                int counter = 0;
                string line;

                System.IO.StreamReader file = new System.IO.StreamReader(script_path);
                while ((line = file.ReadLine()) != null)
                {
                    this.ParseLine(line);
                    counter++;
                }
                file.Close();

            }
            else
            {
                throw new ClientLocalException("Error: The script name you have inserted could not be found!");
            }
            while (true) ;
        }

        private bool ScriptExists(string script_path)
        {
            bool result;

            if (File.Exists(script_path))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private string AssembleScript()
        {
            string current_path;

            current_path = ClientUtils.AssembleCurrentPath() + "\\" + "Scripts" + "\\" + this.script_name;            

            return current_path;
        }
        private void ParseLine(string text_line)
        {
            string[] words = text_line.Split(' ');

            switch(words[0])
            {
                case CREATE:
                    command = new Create(ref this.clientLibrary, words);
                    command.Execute();
                    break;
                case LIST:
                    command = new List(ref this.clientLibrary);
                    command.Execute();
                    break;
                default:
                    Console.WriteLine("Error: You must insert a valid command!");
                    break;
            }
        }
  
    }
}
