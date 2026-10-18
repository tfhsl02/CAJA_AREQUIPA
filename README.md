# Sistema de Gestión Bancaria "Caja Arequipa" en C# (.NET MAUI)

Aplicación para Windows y Android que administra agencias, las conexiones de sincronización entre ellas y el procesamiento de operaciones. Las estructuras de datos principales están implementadas manualmente.

## Arquitectura del proyecto

```
CAJA_AREQUIPA/
│
├── CAJA_AREQUIPA/
│   ├── Modelos/
│   ├── Estructuras/
│   ├── Servicios/
│   ├── Data/
│   ├── Helpers/
│   ├── UI/
│   ├── Resources/
│   ├── Platforms/
│   │
│   ├── MauiProgram.cs
│   ├── App.xaml
│   └── CAJA_AREQUIPA.csproj
│
├── CAJA_AREQUIPA.sln
└── README.md
```

## Descripción de capas

### Modelos
Representan las entidades del sistema:

* Agencia
* Conexion
* Operacion
* RutaSincronizacion

Estas clases contienen únicamente datos del dominio y sus validaciones básicas.

### Estructuras
Estructuras de datos implementadas manualmente:

* ColaOperaciones (cola FIFO de operaciones pendientes)
* PilaReversiones (pila LIFO para revertir operaciones)
* ArbolOperaciones (árbol binario de búsqueda AVL por código)
* GrafoAgencias (grafo ponderado con ruta mínima por Dijkstra)
* ListaSimple (historial de operaciones)
* ListaDoble (bitácora de operaciones procesadas)
* ListaCircular (turnos de sincronización entre agencias)

### Servicios
Contienen la lógica del negocio:

* Registrar agencias
* Registrar conexiones
* Activar o desactivar enlaces
* Registrar operaciones
* Procesar la siguiente operación
* Revertir la última operación
* Buscar operaciones por código
* Calcular la ruta mínima de sincronización

### Data
Responsable de la persistencia en disco utilizando JSON.
La clase AlmacenDatos centraliza:

* Lectura de archivos
* Escritura de archivos
* Serialización
* Deserialización
* Manejo de rutas

### Helpers
Utilidades compartidas por la interfaz:

* Validación de los datos que escribe el usuario
* Formato de montos, fechas y tiempos
* Colores y conversión de modelos a filas de lista

### UI
Contiene toda la interacción con el usuario:

* Menú de navegación (Inicio, Agencias, Conexiones, Operaciones, Búsqueda y Rutas)
* Lectura de datos mediante formularios
* Mensajes al usuario

## Cómo ejecutar el proyecto

### Requisitos

* .NET SDK 9 instalado
* Carga de trabajo de .NET MAUI
* Para Android: Android SDK y un emulador o un celular con depuración USB

Verificar instalación:

```
dotnet --version
```

Instalar la carga de trabajo de MAUI (si no la tienes):

```
dotnet workload install maui
```

### Ejecutar

Desde Visual Studio: abrir `CAJA_AREQUIPA.sln`, elegir **Windows Machine** o **Android Emulator** y presionar F5.

Desde la carpeta del proyecto (`CAJA_AREQUIPA/CAJA_AREQUIPA`):

```
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

Para Android:

```
dotnet build -t:Run -f net9.0-android
```

## Datos

Los archivos `agencias.json`, `conexiones.json` y `operaciones.json` se guardan en la carpeta privada de la aplicación. La ruta exacta aparece en la sección Inicio.
