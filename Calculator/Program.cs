using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;

namespace CalculatorApp
{


   

    public class Calculator
    {
        public int Calculate(string operation, int a, int b)
        {
           
            switch (operation)
            {
                case "Add" : return a + b;
                case "Subtract": return a - b;
                case "Multiply": return a * b;
            }
            return 0;
        }
    }

    public interface ICalculatorOperation
    {
        string Operation();
        int Calculate(int a, int b);
    }

    public class OperationAdd : ICalculatorOperation
    {
        private readonly string operationName = "Add";
        public string Operation()
        {
            return operationName;
        }

        public int Calculate(int a, int b)
        {
            return a + b;
        }
    }

    public class OperationSubtract : ICalculatorOperation
    {
        private readonly string operationName = "Subtract";
        public string Operation()
        {
            return operationName;
        }
 
        public int Calculate(int a, int b)
        {
            return a - b;
        }
    }

    public class OperationMultiply : ICalculatorOperation
    {
        private readonly string operationName = "Multiply";
        public string Operation()
        {
            return operationName;
        }
 
        public int Calculate(int a, int b)
        {
            return a * b;
        }
    }

    public class PerformOperation
    {
        private readonly IEnumerable<ICalculatorOperation> _operations;

        public PerformOperation(IEnumerable<ICalculatorOperation> operations)
        {
            _operations = operations;
        }

        public int Operate(string operation, int a, int b)
        {
            var oper = _operations.SingleOrDefault(o => o.Operation().ToLower() == operation.ToLower());
            if (oper == null) throw  new Exception($"Operation {operation} is not supported");
            return oper.Calculate(a, b);
        }
    }

    class Program
    {
        static void Main(string[] args)
{
            Console.WriteLine("App started");

            // Create your builder.
            var builder = new ContainerBuilder();

            //builder.RegisterType<OperationAdd>().As<ICalculatorOperation>();
            //builder.RegisterType<OperationSubtract>().As<ICalculatorOperation>();
            //builder.RegisterType<OperationMultiply>().As<ICalculatorOperation>();


            builder.RegisterAssemblyTypes(typeof(Calculator).Assembly).Where(x => x.Name.StartsWith("Operation"))
            .AsImplementedInterfaces();
            var appContainer = builder.Build();
            var operations = appContainer.Resolve<IEnumerable<ICalculatorOperation>>();


            //List<ICalculatorOperation> operations = new List<ICalculatorOperation>
            //{
            //    new OperationAdd(),
            //    new OperationSubsctract(),
            //    new OperationMultiply()
            //};

            var sum = new PerformOperation(operations).Operate("Multiply", 3, 4);
 
            

        }
    }
}
