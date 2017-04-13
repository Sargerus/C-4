using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

delegate void D(Object obj, MyEvents me);
class MyEvents : EventArgs
{
    public int voltage;
    public MyEvents(int vol) { voltage = vol; }
}
class Events
{
    public Events() { }
    public event D userEvents;
    public void onEvents(int n)
    {
        userEvents(this,new MyEvents(n));
        Console.WriteLine("---------------------------------");
    }
}
sealed class Robot
{
    static int robotsBound = 100;
    private int voltage;
    private string name;
    private int countOf;
    public bool isDead;
    public bool test;
    
    public Robot(int volt,string nam) 
    {
        voltage = volt;
        name = nam;
        countOf = 0;
    }
    public int Volt {
        get { return voltage; }
        set { voltage = value; }
}
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public void checkVoltage(Object obj, MyEvents me)
    {
        if (countOf == 3) { countOf++; Console.WriteLine("Too much voltage!!! Robot {0} is being exploded!!!", name); return; }
        if (countOf < 3)
        {
            if (voltage > robotsBound)
            {
                countOf++;
                Console.WriteLine("Robot {0} is now dead.", name);
                return;
            }
            voltage += me.voltage;
            if (voltage > 100)
                Console.WriteLine("Robot {0} is out of work", name);
            else
            Console.WriteLine("Robot {0} is ok, his voltage now is {1}", name, voltage);
        }
        else Console.WriteLine("This robot {0} doesnt exist",name);

        Console.WriteLine();
    }
    public void checkBoss(Object obj, MyEvents me)
    {
        if (me.voltage < 100)
            Console.WriteLine("Not enougth voltage for boss {0}", name);
        else Console.WriteLine("{0} is now ready",name);
    }
    public void DegreeVoltage(int n)
    {
        voltage -= n;
        Console.WriteLine("Voltage of {0} is now {1}", name, voltage);
    }
     
}

static class Reflector
{
   // public Reflector() { }
    public static void ShowInfoAboutClass(string className,string parameterType, object obj){
        Type t = Type.GetType(className, false, true);
        short n = 0;

        Console.WriteLine("\n\nType name is {0}", t.Name); 
        //public methods
        Console.WriteLine("----------Public methods----------");
        var u = t.GetMethods();
        var publicMethods = from g in t.GetMethods() where g.IsPublic == true select g.Name;
        foreach (string g in publicMethods)
        {
            Console.WriteLine("{0}){1}",n+1,g);
            n++;
        }
        //properties
        n = 0;
        Console.WriteLine("----------Properties----------");
        var properties = from g in t.GetProperties() select g.Name;
        foreach (var g in properties)
        {
            Console.WriteLine("{0}){1}", n + 1, g);
            n++;
        }
        //fields
        n = 0;

        Console.WriteLine("----------Fields----------");
        var fields = from g in t.GetFields() select g.Name;
        foreach (var g in fields)
        {
            Console.WriteLine("{0}){1}", n + 1, g);
            n++;
        }
        //interfaces
        n = 0;

        Console.WriteLine("----------Interfaces----------");
        var ifaces = from g in t.GetInterfaces() select g;
        foreach (var g in ifaces)
        {
            Console.WriteLine("{0}){1}", n + 1, g.Name);
            n++;
        }
        //parameterType
        n = 0;
        Console.WriteLine("----------Method with return parameterType----------");
        var mypar = from g in t.GetMethods() where g.ReturnType.FullName == parameterType select g.Name;
        foreach (var g in mypar)
        {
            Console.WriteLine("{0}){1}", n + 1, g);
            n++;
        }
        //dynamic link
        string arg = File.ReadAllText(@"e:\4 семестр\c#\4\arguments.txt");
        int intArg = Int32.Parse(arg);
        

        //object obj = Activator.CreateInstance(t);
        //Console.WriteLine("{0} was created", obj);


        Console.WriteLine("Voltage is {0}", ((Robot)obj).Volt);
        MethodInfo method = t.GetMethod("DegreeVoltage");
        method.Invoke(obj, new object[] { intArg });
    }
}
class Program
    {
        static void Main(string[] args)
        {
            
            Robot r1 = new Robot(62,"Jack");
            Robot r2 = new Robot(32,"Simon");
            Robot r3 = new Robot(99,"George");
            Robot boss = new Robot(0, "Boss");
            Events ev = new Events();

            ev.userEvents += r1.checkVoltage;
            ev.userEvents += r2.checkVoltage;
            ev.userEvents += r3.checkVoltage;
            ev.userEvents += boss.checkBoss;

            //ev.userEvents = () => Console.WriteLine("asd");  

            ev.onEvents(20);
            ev.onEvents(20);
            ev.onEvents(20);
            ev.onEvents(20);
            ev.onEvents(101);

            Reflector.ShowInfoAboutClass("Robot","System.Void",r1);
           // Reflector.ShowInfoAboutClass("System.Int32", "System.Void", r1);
            
        }
    }

