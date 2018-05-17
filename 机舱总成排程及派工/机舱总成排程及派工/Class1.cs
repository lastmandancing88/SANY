using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 机舱总成排程及派工
{
    public partial class Worker
    {
        public Worker()
        {
            Name = "";
            Schedule = "白班";
        }
        public Worker(string name) : this()
        {
            Name = name;
        }
        public Worker(string name, string schedule)
        {
            Name = name;
            Schedule = schedule;
        }
        public string Name { get; set; }
        public string Schedule { get; set; }

    }
    public partial class Worker : IEquatable<Worker>
    {
        public bool Equals(Worker other)
        {
            if (this.Name == other.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class Step
    {
        public string ProcessName { get; set; }
        public string StepNumber { get; set; }
        public string StepName { get; set; }
        public double WorkingTime { get; set; }
        public double FinishTime { get; set; }
        public bool IsCoorperate { get; set; }
        public Step()
        {
            ProcessName = "";
            StepNumber = "";
            StepName = "";
            WorkingTime = 0;
            FinishTime = 0;
            IsCoorperate = false;
        }
        public Step(string processName, string stepNumber, string stepName, double workingTime)
            : this()
        {
            ProcessName = processName;
            IsCoorperate = stepNumber[0] == 1 ? true : false;
            StepNumber = stepNumber;
            StepName = stepName;
            WorkingTime = workingTime;
        }
        public Step(string processName, string stepNumber, string stepName, double workingTime, double finishTime)
            : this(processName, stepNumber, stepName, workingTime)
        {
            FinishTime = finishTime;
        }
    }
    public class Schedule
    {
        public Worker Worker;
        public List<Step> Steps;

        public Schedule()
        {
            Worker = new Worker();
            Steps = new List<Step>();
        }
        public Schedule(Worker worker) : this()
        {
            this.Worker = worker;
        }

        public Schedule(Worker worker, Step step) : this(worker)
        {
            Steps.Add(step);
        }
    }
    public partial class WaitTime
    {
        public string Name { get; set; }
        public double Time { get; set; }
        public WaitTime()
        {
            Name = "";
            Time = 0;
        }
        public WaitTime(string name) : this()
        {
            Name = name;
        }
        public WaitTime(string name, double time) : this(name)
        {
            Time = time;
        }
    }
    public partial class WaitTime : IEquatable<WaitTime>
    {
        public bool Equals(WaitTime other)
        {
            if (this.Name == other.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
