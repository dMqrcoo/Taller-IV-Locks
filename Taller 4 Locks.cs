using System;
using System.Collections.Generic;
using System.Threading;

public class Atraccion
{
    private readonly string nombre;
    private readonly int capacidadMaxima;
    private readonly List<int> visitantesEnAtraccion = new List<int>(); 
    private readonly object _lock = new object(); //Con un lock sincronizamos el acceso a la atraccion

    public Atraccion(string nombre, int capacidadMaxima)
    {
        this.nombre = nombre;
        this.capacidadMaxima = capacidadMaxima;
    }

    public bool IntentarSubir(int idVisitante) //Con este metodo se suben los visitantes a la atraccion
    {
        bool entro = false;

        if (Monitor.TryEnter(_lock, TimeSpan.FromMilliseconds(500)))
        {
            try
            {
                if (visitantesEnAtraccion.Count < capacidadMaxima) //Con un if verificamos que la atraccion no este a capacidad maxima para que el visitante se pueda subir
                {
                    visitantesEnAtraccion.Add(idVisitante);
                    Console.WriteLine($"Visitante {idVisitante} se subió a {nombre}.");
                    entro = true;
                }
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        return entro;
    }

    public void Bajar(int idVisitante)
    {
        lock (_lock)
        {
            if (visitantesEnAtraccion.Contains(idVisitante))
            {
                visitantesEnAtraccion.Remove(idVisitante);
                Console.WriteLine($"Visitante {idVisitante} bajó de {nombre}.");
            }
        }
    }

    public string Nombre => nombre;
}



public class Visitante
{
    private readonly int id;
    private readonly Atraccion[] atracciones;
    private readonly Random random = new Random();

    public Visitante(int id, Atraccion[] atracciones)
    {
        this.id = id;
        this.atracciones = atracciones;
    }

    public void Visitar()
    {
        for (int i = 0; i < 3; i++) // Con un for nos aseguramos que cada visitante se suba a 3 atracciones
        {
            Atraccion atraccion = atracciones[random.Next(atracciones.Length)];//Con el metodo random se selecciona una atraccion aleatoria para el visitante

            if (atraccion.IntentarSubir(id))
            {
                Thread.Sleep(random.Next(1000, 2000)); //Con un thread sleep definimos el tiempo que estaran en la atraccion los visitantes
                atraccion.Bajar(id);//El visitante se baja de la atraccion
            }
            else
            {
                Console.WriteLine($"Visitante {id} no pudo entrar a {atraccion.Nombre} y se va a otra atracción.");//Si el visitante no pudiese entrar, esperara un rato antes de intentar subir a otra
                Thread.Sleep(500); // Se simula un tiempo de espera antes de intentar subir a otra 
            }
        }
    }
}



class Program
{
    static void Main(string[] args)
    {
        Atraccion[] atracciones = new Atraccion[] //Se definen las atracciones y su respectiva capacidad
        {
            new Atraccion("Montaña Rusa", 3),
            new Atraccion("Noria", 2),
            new Atraccion("Carrusel", 4)
        };

        Thread[] hilos = new Thread[10];//Se crean los hilos para los visitantes

        for (int i = 0; i < hilos.Length; i++)
        {
            int id = i; 
            hilos[i] = new Thread(() =>
            {
                Visitante v = new Visitante(id, atracciones);
                v.Visitar();
            });
            hilos[i].Start();
        }

        foreach (var hilo in hilos)//Se espera que todos los hilos terminen antes de finalizar la simulación
            hilo.Join();

        Console.WriteLine("Simulación completada.");
    }
}
