# Skybnb

## Tabla de contenido

1. <a href="#description">Descripción</a>
1. <a href="#configuration">Configuración de Entorno</a>
1. <a href="#frontend-configuration">Configuración del Frontend</a>

<h2 id="prerequisites">Prerequisitos</h2>

Asegúrate de tener instalado el siguiente software:

- SDK de .NET ([.NET 8.0 o superior](https://dotnet.microsoft.com/en-us/download))
- Node.js ([v18.0 o superior](https://nodejs.org/)) y npm
- Git
- Visual Studio 2022 o superior
- OpenSSL para la generación de claves
  - Usualmente preinstalado en Linux/macOS
  - En Windows puede obtenerse de https://slproweb.com/products/Win32OpenSSL.html o mediante WSL/Git Bash
- SQL Server (Express o superior)

<h2 id="description">Descripción</h2>

Plataforma de alquiler a corto plazo de propiedades que permite a Anfitriones publicar propiedades y a Huéspedes reservar y calificar su experiencia.

<h2 id="configuration">Configuración e Inicio del Proyecto .NET</h2>

### Variables de entorno

1. En Visual Studio:

   - Haz clic derecho en el proyecto `repository` en el Explorador de Soluciones.
   - Selecciona "Administrar secretos de usuario". Esto abrirá (o creará) el archivo `secrets.json` asociado a este proyecto.

1. Si no usas Visual Studio, puedes inicializar los secretos de usuario desde la terminal en la carpeta del proyecto "repository":
   ```Powershell
   dotnet user-secrets init --project .
   ```
   Y luego editar el archivo `secrets.json` que se crea en la ruta de perfil de usuario (la ruta se mostrará al ejecutar `dotnet user-secrets list`).
1. Asegúrate de que tu archivo `secrets.json` tenga la siguiente estructura y define los valores necesarios:
   ```json
   {
     "ConnectionStrings": {
       "ConexionString": "TU_CADENA_DE_CONEXION_A_LA_BASE_DE_DATOS_AQUI"
     },
     "JwtOptions": {
       "SecretKey": "TU_CLAVE_SECRETA_DE_AL_MENOS_32_BYTES_AQUI",
       "TokenExpirationInMinutes": 30
     }
     // Puedes añadir otras configuraciones sensibles aquí
   }
   ```
1. Definición de las variables:

   - `JwtOptions:SecretKey`:

     - **Propósito**: Clave utilizada para la firma digital de JSON Web Tokens (JWTs). Es crucial para la seguridad de la autenticación.
     - **Requisito**: Debe ser una cadena criptográficamente fuerte. Se recomienda una entropía de al menos 32 bytes.
     - **Generación (Recomendado)**: Puedes generar una clave adecuada usando OpenSSL en tu terminal:
       ```bash
       openssl rand -base64 32
       ```
       Copia la salida de este comando y p�gala como valor para `SecretKey`.
     - **¡Importante!**: Esta clave debe mantenerse secreta. La generada aquí es para desarrollo local. Producción y otros entornos deben tener sus propias claves únicas y seguras gestionadas adecuadamente. - `JwtOptions:TokenExpirationInMinutes`:

     - **Propósito**: Define la duración en minutos antes de que expire un token JWT.
     - **Valor por defecto**: 30 minutos es un valor común para desarrollo.
     - **Consideración**: Para producción, evalúa si este tiempo es adecuado según los requisitos de seguridad y experiencia de usuario de tu aplicación. - `ConnectionStrings:ConexionString`:
     - **Propósito**: Cadena de conexión para la base de datos principal de la aplicación.
     - **Ejemplos**:
       - SQL Server LocalDB (típico en instalaciones de Visual Studio):
         ```txt
         Server=(localdb)\\mssqllocaldb;Database=NombreTuBaseDeDatos_Dev;Trusted_Connection=True;MultipleActiveResultSets=true
         ```
       - SQL Server Express:
         ```txt
         Server=.\\SQLEXPRESS;Database=NombreTuBaseDeDatos_Dev;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
         ```
         Considera `TrustServerCertificate=True` solo para desarrollo local si usas un certificado autofirmado.
       - PostgreSQL:
         ```txt
         Host=localhost;Port=5432;Database=NombreTuBaseDeDatos_Dev;Username=tu_usuario;Password=tu_contraseña
         ```

#### Nota sobre otros entornos

Para entornos como Staging o Producción, no se utilizan los `secrets.json`. En su lugar, la configuración se gestiona a través de variables de entorno del sistema operativo, servicios de configuración en la nube (como Azure App Configuration, AWS Systems Manager Parameter Store) o bóvedas de secretos (como HashiCorp Vault o Azure Key Vault).

### Inicialización de la base de datos

Este proyecto utiliza Entity Framework Core para la gestión de la base de datos. Las migraciones definen el esquema de la base de datos.

1. Abrir la Consola del Administrador de Paquetes en Visual Studio:
   - ve a `Ver -> Otras Ventanas -> Consola del Administrador de Paquetes`.
1. Configurar Proyectos en la Consola:
   - Asegúrate de que el "Proyecto predeterminado" en la Consola del Administrador de Paquetes esté configurado como `repository` (o el proyecto que contiene tus `DbContext` y Migraciones).
1. Ejecutar Migraciones:
   - Ejecuta el siguiente comando para aplicar las migraciones pendientes y crear/actualizar el esquema de tu base de datos:
     ```Powershell
     Update-Database
     ```
1. Solución de Problemas Comunes:
   - `Error Unable to create an object of type 'YourDbContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728` o `Not found repository.dll` (o similar):
     - Las herramientas de Entity Framework Core a veces necesitan un proyecto de inicio (startup project) que pueda configurar y proporcionar los servicios necesarios (como el `DbContext` y la cadena de conexión).
     - **Solución**: En Visual Studio, haz clic derecho en el `proyecto asp_services` (o tu proyecto API/Web principal) en el Explorador de Soluciones y selecciona "Establecer como proyecto de inicio". Luego, intenta ejecutar `Update-Database` nuevamente.
     - **Alternativa desde CLI (dotnet-ef)**: Si prefieres la línea de comandos, puedes ejecutar (desde la raíz del repositorio o la carpeta de solución):
       ```Powershell
       dotnet ef database update --project "ruta/al/proyecto/repository" --startup-project "ruta/al/proyecto/asp_services"
       ```

### Ejecutar la Aplicación

Una vez configurado el entorno y la base de datos, puedes iniciar la aplicación.

#### Desde Terminal (CLI)

1. Abre una terminal o Símbolo del sistema.
1. Navega a la carpeta raíz del proyecto `asp_services` (o tu proyecto API/Web principal).
   ```Powershell
   cd ruta/a/tu/proyecto/asp_services
   ```
1. Ejecuta el siguiente comando para compilar e iniciar la aplicación:
   ```Powershell
   dotnet run
   ```
1. La aplicación generalmente estará disponible en `http://localhost:5299` y/o `https://localhost:7241` (o los puertos configurados en `Properties/launchSettings.json` del `proyecto asp_services`). Revisa la salida de la consola para ver las URLs exactas.

#### Desde Visual Studio

1. Establecer Proyecto de Inicio:
   - En el Explorador de Soluciones, haz clic derecho en el proyecto `asp_services` (o tu proyecto API/Web principal).
   - Selecciona "Establecer como proyecto de inicio".
1. Seleccionar Perfil de Inicio:
   - En la barra de herramientas superior, junto al botón de inicio (flecha verde), verás un menú desplegable con perfiles de inicio (ej: `http`, `https`, `IIS Express`). Selecciona el perfil deseado (generalmente `http` o `https` para desarrollo). Estos perfiles están definidos en `Properties/launchSettings.json`.
1. Ejecutar:
   - Presiona el botón de inicio (flecha verde) o `F5`.
1. Visual Studio abrirá automáticamente un navegador en la URL configurada para el perfil seleccionado.

<h2 id="frontend-configuration">Configuración del Frontend</h2>

El frontend de la aplicación está desarrollado con React, TypeScript y Vite, ubicado en la carpeta `web_presentation/Spa`.

### Prerequisitos del Frontend

- Node.js v18.0 o superior
- npm (incluido con Node.js)

### Configuración de Variables de Entorno del Frontend

1. **Crear archivo de configuración**:

   - Navega a la carpeta `web_presentation/Spa`.
   - Encontrarás un archivo llamado `.env.rename` que contiene las variables de entorno necesarias.
   - Crea una copia de este archivo y renómbralo a `.env`:
     ```Powershell
     cd web_presentation\Spa
     Copy-Item ".env.rename" ".env"
     ```

1. **Configurar las variables de entorno**:
   Edita el archivo `.env` y define los valores necesarios:

   ```bash
   # URL base de la API del backend
   VITE_API_URL=http://localhost:5299

   # Configuración de Firebase (para autenticación y almacenamiento)
   VITE_FIREBASE_FIREBASE_API_KEY=tu_firebase_api_key
   VITE_FIREBASE_AUTH_DOMAIN=tu_proyecto.firebaseapp.com
   VITE_FIREBASE_PROJECT_ID=tu_proyecto_id
   VITE_FIREBASE_STORAGE_BUCKET=tu_proyecto.appspot.com
   VITE_FIREBASE_MESSAGING_SENDER_ID=tu_sender_id
   VITE_FIREBASE_APP_ID=tu_app_id
   VITE_FIREBASE_MEASUREMENT_ID=tu_measurement_id
   VITE_FIREBASE_BUCKET_MULTIMEDIA=tu_bucket_multimedia
   ```

1. **Definición de las variables**:
   - `VITE_API_URL`: URL base donde se ejecuta tu API backend. Para desarrollo local, generalmente es `http://localhost:5299` o `https://localhost:7241`.
   - Variables de Firebase: Configuración necesaria para la integración con Firebase (autenticación, almacenamiento de archivos, etc.). Obtén estos valores desde tu consola de Firebase.

### Instalación de Dependencias

1. **Instalar paquetes npm**:
   ```Powershell
   cd web_presentation\Spa
   npm install --legacy-peer-deps # Debido a posibles conflictos de dependencias con React 19.0
   ```

### Ejecutar el Frontend

#### Modo Desarrollo

1. Al iniciar el proyecto, asegúrate de que el backend esté en ejecución.
2. El frontend se ejecuta en paralelo con la aplicación de Razor Pages. Entonces, no es necesario iniciar un servidor separado.
3. Para iniciar la aplicación Razor Pages, abre una terminal en la carpeta `web_presentation` y ejecuta:

```Powershell
   dotnet run
```

5. Abre tu navegador y navega a `http://localhost:6299` o `https://localhost:7241` (según la configuración de tu proyecto).
6. Al compilar e iniciar el proyecto, Vite se encargará de servir el frontend y manejar la recarga en caliente (HMR).

### Estructura del Proyecto Frontend

```
/web_presentation/
├── package.json           # Dependencias y scripts de npm
|-- package-lock.json      # Bloqueo de versiones de dependencias
├── vite.config.ts         # Configuración de Vite
├── tsconfig.json          # Configuración de TypeScript
└── tailwind.config.js     # Configuración de Tailwind CSS
	Spa/
	├── src/                    # Código fuente de la aplicación
	├── public/                 # Archivos estáticos públicos
	├── .env                   # Variables de entorno (no incluir en git)
	├── .env.rename            # Plantilla de variables de entorno
```

### Notas Importantes

- **Seguridad**: Nunca incluyas el archivo `.env` en el control de versiones. Está incluido en `.gitignore`.
- **Sincronización con Backend**: Asegúrate de que la `VITE_API_URL` coincida con la URL donde se ejecuta tu backend.
- **Firebase**: Si no planeas usar Firebase, puedes dejar las variables de Firebase vacías, pero es recomendable configurarlas para el funcionamiento completo de la aplicación.
