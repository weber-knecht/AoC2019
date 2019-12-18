using NUnit.Framework;
using Intcode;
using System;
using System.Collections.Generic;
using System.IO;

namespace Intcode.Test
{
    [TestFixture]
    public class IntcodeComputerTests
    {
        private static List<int> ReadFileBySeperator(string location) {
            List<int> input = new List<int>();
            StreamReader reader = new StreamReader(location);
            string line = String.Empty;
            while((line = reader.ReadLine()) != null) {
                string[] s = line.Split(',');
                for(int i=0; i<s.Length; i++) {
                    input.Add(int.Parse(s[i]));
                }
            }
            return input;
        }

        private List<int> input;
        private List<int> complexInput;
        private List<int> negativeInput;
        private List<int> instructionInput;

        [SetUp]
        public void Setup()
        {
            //1,9,10,3,2,3,11,0,99,30,40,50
            this.input = new List<int>();
            this.input.Add(1);
            this.input.Add(9);
            this.input.Add(10);
            this.input.Add(3);
            this.input.Add(2);
            this.input.Add(3);
            this.input.Add(11);
            this.input.Add(0);
            this.input.Add(99);
            this.input.Add(30);
            this.input.Add(40);
            this.input.Add(50);   

            // 1002,4,3,4,33
            instructionInput = new List<int>();
            instructionInput.Add(1002);
            instructionInput.Add(4);
            instructionInput.Add(3);
            instructionInput.Add(4);
            instructionInput.Add(33);

            negativeInput = new List<int>();
            negativeInput.Add(1101);
            negativeInput.Add(100);
            negativeInput.Add(-1);
            negativeInput.Add(4);
            negativeInput.Add(0);

            this.complexInput = ReadFileBySeperator(".//input");
        }

        [Test]
        public void IntcodeExecuteOPCode1()
        {
            Computer intcode = new Computer(this.input, 9, 10);
            List<int> result = intcode.Execute();
            Assert.AreEqual(70, result[3], 0, "Value should be "+ 70 + " but is " + result[3]);
        }

        [Test]
        public void IntcodeExecuteOPCode2()
        {
            Computer intcode = new Computer(this.input, 9, 10);
            List<int> result = intcode.Execute();
            Assert.AreEqual(result[0], 3500, 0, "Value should be "+ 3500 + " but is " + result[0]);
        }

        [Test]
        public void IntcodeExecuteOPCode3() {
            List<int> opcode3TestInput = new List<int>();
            opcode3TestInput.Add(3);
            opcode3TestInput.Add(5);
            opcode3TestInput.Add(0);
            opcode3TestInput.Add(0);
            opcode3TestInput.Add(99);
            opcode3TestInput.Add(42);
            Computer intcode = new Computer(opcode3TestInput, 0 ,0);
            List<int> result = intcode.Execute(24);
            Assert.AreEqual(24, result[5], 0, "Value should be "+ 24 + " but is " + result[5]);
        }

        [Test]
        public void IntcodeExecuteOPCode4() {
            List<int> opcode4TestInput = new List<int>();
            opcode4TestInput.Add(4);
            opcode4TestInput.Add(5);
            opcode4TestInput.Add(0);
            opcode4TestInput.Add(0);
            opcode4TestInput.Add(99);
            opcode4TestInput.Add(42);
            Computer intcode = new Computer(opcode4TestInput, 0 ,0);
            List<int> result = intcode.Execute();
            Assert.AreEqual(42, intcode.Outputs[0], 0, "Value should be "+ 42 + " but is " + intcode.Outputs[0]);
        }

        [Test]
        public void IntcodeExecuteVerbNounProcessing() {
            Computer intcode = new Computer(this.complexInput, 33, 76);
            List<int> result = intcode.Execute();
            Assert.AreEqual(19690720, result[0], 0, "Value should be "+ 19690720 + " but is " + result[0]);
        }

        [Test]
        public void IntcodeExecuteVerbNounProcessing2() {
            Computer intcode = new Computer(this.complexInput, 12, 2);
            List<int> result = intcode.Execute();
            Assert.AreEqual(7594646, result[0], 0, "Value should be "+ 7594646 + " but is " + result[0]);
        }

        [Test]
        public void InstructionCreation() {
            Computer intcode = new Computer(this.instructionInput, 0, 0);
            Instruction newInstruction = intcode.CreateInstruction(0);
            Assert.AreEqual(newInstruction.OpCode, OPCODE.multiply);
            Assert.AreEqual(newInstruction.Modes[0], MODE.position);
            Assert.AreEqual(newInstruction.Modes[1], MODE.immediate);
            Assert.AreEqual(newInstruction.Modes[2], MODE.position);
            Assert.AreEqual(4, newInstruction.Parameters[0]);
            Assert.AreEqual(3, newInstruction.Parameters[1]);
            Assert.AreEqual(4, newInstruction.Parameters[2]);
            //Assert.AreEqual(33, newInstruction.Parameters[3]);
        }
 
        [Test]
        public void HandleNegativIntegers() {
            Computer intcode = new Computer(this.negativeInput, 0, 0);
            List<int> result = intcode.Execute();
            Assert.AreEqual(99, result[4]);
        }

    }
}