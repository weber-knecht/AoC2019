using System.Collections.Generic;

namespace Intcode
{
    public class Instruction
    {
       public Intcode.OPCODE OpCode { get; set; }
       
       public List<Intcode.MODE> Modes {get; set; }

       public List<int> Parameters { get; set; }

       public int InstructionPointer {
           get {
                return (this.Parameters.Count + 1);
           }
       }

       public Instruction() {
           this.OpCode = OPCODE.finish;
           this.Modes = new List<MODE>();
           Parameters = new List<int>();
       }

       public Instruction(OPCODE opCODE, List<MODE> modes, List<int> parameters) {
           this.OpCode = opCODE;
           this.Modes = modes;
           this.Parameters = parameters;
       }
    }
}