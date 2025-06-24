# Taller-IV-Locks

Simulación de Parque de Atracciones en C#
El objetivo de este taller es el de simular en consola desarrollada en C# que representa un parque de atracciones donde múltiples visitantes que serán simulados con hilos intentan ingresar a diferentes atracciones, los visitantes deben en todo momento deben respetar la capacidad máxima y se utiliza sincronización para evitar “colisiones”.

Estructura del Proyecto
`Atraccion.cs`: Representará las atracciones del parque. Controlará el acceso de visitantes mediante `Monitor.TryEnter` y respeta la capacidad máxima.
`Visitante.cs`: Simulará a un visitante que intenta usar aleatoriamente diferentes atracciones.
`Program.cs`: En program.cs, se definen las atracciones y a su vez los hilos de los visitantes

 

 



