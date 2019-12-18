using System;
using System.Collections.Generic;

namespace Intcode
{
    public class Computer
    {
        public List<int> Memory { get; set; }

        public int Noun {get; set; }

        public int Verb { get; set; }
        public List<int> Outputs { get; set; }
        private const int INPUT_ADDRESS_1 = 1;
        private const int INPUT_ADDRESS_2 = 2;

        public Computer(List<int> memory, int noun, int verb) {
            this.Memory = memory;
            this.Noun = noun;
            this.Verb = verb;

            if (this.Noun != 0) 
                this.Memory[INPUT_ADDRESS_1] = this.Noun;

            if (this.Verb != 0)    
                this.Memory[INPUT_ADDRESS_2] = this.Verb;

            Outputs = new List<int>();
        }

        public Computer(): this(new List<int>(), 0,0) {
            this.Outputs = new List<int>();
        }

        public Instruction CreateInstruction(int positionPointer) {
            int memoryValue = this.Memory[positionPointer];

            string opCodeMode = memoryValue.ToString("D5");

            OPCODE code = (OPCODE)int.Parse(opCodeMode.Substring(3,2));
            List<MODE> modes = new List<MODE>();

            MODE modeParam1;
            MODE modeParam2;
            MODE modeParam3;

            List<int> parameters = new List<int>();

            switch (code)
            {
                case OPCODE.add:
                    modeParam1 = (MODE)int.Parse(opCodeMode.Substring(2,1));
                    modeParam2 = (MODE)int.Parse(opCodeMode.Substring(1,1));
                    modeParam3 = (MODE)int.Parse(opCodeMode.Substring(0,1));

                    modes.Add(modeParam1);
                    modes.Add(modeParam2);
                    modes.Add(modeParam3);

                    parameters.Add(this.Memory[positionPointer+1]);
                    parameters.Add(this.Memory[positionPointer+2]);
                    parameters.Add(this.Memory[positionPointer+3]);
                    break;
                case OPCODE.multiply:
                    modeParam1 = (MODE)int.Parse(opCodeMode.Substring(2,1));
                    modeParam2 = (MODE)int.Parse(opCodeMode.Substring(1,1));
                    modeParam3 = (MODE)int.Parse(opCodeMode.Substring(0,1));

                    modes.Add(modeParam1);
                    modes.Add(modeParam2);
                    modes.Add(modeParam3);

                    parameters.Add(this.Memory[positionPointer+1]);
                    parameters.Add(this.Memory[positionPointer+2]);
                    parameters.Add(this.Memory[positionPointer+3]);
                    break;
                case OPCODE.Input:
                    modeParam1 = (MODE)int.Parse(opCodeMode.Substring(2,1));

                    modes.Add(modeParam1);

                    parameters.Add(this.Memory[positionPointer+1]);        
                    break;
                case OPCODE.Output:
                    modeParam1 = (MODE)int.Parse(opCodeMode.Substring(2,1));

                    modes.Add(modeParam1);

                    parameters.Add(this.Memory[positionPointer+1]);
                    break;
                case OPCODE.finish:
                    break;
                default:
                    throw new ArgumentException("Create Instruction: Unkown IntCode: "+ code.ToString());
            }
           
            return new Instruction(code, modes, parameters);
        }

        private int HandleMode(MODE mode, int parameter) {
            int returnValue = 0;
            switch (mode)
            {
                case  MODE.position:
                    returnValue = this.Memory[parameter];
                    break;
                case MODE.immediate:
                    returnValue = parameter;
                    break;
                default:
                    throw new ArgumentException("Unknown Mode: "+mode.ToString());
            }
            return returnValue;
        }

        public List<int> Execute() {
            return this.Execute(0);
        }

        public List<int> Execute(int computerInput) {
            int step;
            for(int i = 0; i < this.Memory.Count; i+=step) {
                try {
                    Instruction instruction = this.CreateInstruction(i);
                    step = instruction.InstructionPointer;
                    switch (instruction.OpCode)
                    {
                        case OPCODE.add:
                            this.Memory[instruction.Parameters[2]] = this.HandleMode(instruction.Modes[0], instruction.Parameters[0]) 
                                                                   + this.HandleMode(instruction.Modes[1], instruction.Parameters[1]);
                            break;
                        case OPCODE.multiply:
                            this.Memory[instruction.Parameters[2]] = this.HandleMode(instruction.Modes[0], instruction.Parameters[0])
                                                                   * this.HandleMode(instruction.Modes[1], instruction.Parameters[1]);
                            break;
                        case OPCODE.Input:
                            this.Memory[instruction.Parameters[0]] = computerInput;
                            break;
                        case OPCODE.Output:
                            int output = this.HandleMode(instruction.Modes[0], instruction.Parameters[0]);
                            Outputs.Add(output);
                            break;
                        case OPCODE.finish:
                            return this.Memory;
                        default:
                            throw new System.Exception("Unkown OPCODE");
                    }                   
                }
                catch (Exception ex) {
                    throw new Exception(i+":"+this.Memory[1]+":"+this.Memory[2], ex);
                }

            }
            return this.Memory;
        }
    }

    public enum MODE {
        position = 0,
        immediate = 1
    }    

    public enum OPCODE {
        add = 1,
        multiply = 2,
        Input = 3,
        Output = 4,
        finish = 99
    }        
}
