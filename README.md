# Skybnb

## Tabla de contenido

1. <a href="#description">Descripci�n</a>
1. <a href="#configuration">Configuraci�n de Entorno</a>
1. <a href="#frontend-configuration">Configuraci�n del Frontend</a>

<h2 id="prerequisites">Prerequisitos</h2>

Aseg�rate de tener instalado el siguiente software:

- SDK de .NET ([.NET 8.0 o superior](https://dotnet.microsoft.com/en-us/download))
- Node.js ([v18.0 o superior](https://nodejs.org/)) y npm
- Git
- Visual Studio 2022 o superior
- OpenSSL para la generaci�n de claves
  - Usualmente preinstalado en Linux/macOS
  - En Windows puede obtenerse de https://slproweb.com/products/Win32OpenSSL.html o mediante WSL/Git Bash
- SQL Server (Express o superior)

<h2 id="description">Descripci�n</h2>

Plaforma de alquiler a corto plazo de propiedades que permite a Anfitriones publicar propiedades y a Huespedes reservar y calificar su experiencia.

<h2 id="configuration">Configuraci�n e Inicio del Proyecto .NET</h2>

### Variables de entorno

1. En Visual Studio:

   - Haz clic derecho en el proyecto `repository` en el Explorador de Soluciones.
   - Selecciona "Administrar secretos de usuario". Esto abrir� (o crear�) el archivo `secrets.json` asociado a este proyecto.

1. Si no usas Visual Studio, puedes inicializar los secretos de usuario desde la terminal en la carpeta del proyecto �repository�:
   ```Powershell
   dotnet user-secrets init --project .
   ```
   Y luego editar el archivo `secrets.json` que se crea en la ruta de perfil de usuario (la ruta se mostrar� al ejecutar `dotnet user-secrets list`).
1. Aseg�rate de que tu archivo `secrets.json` tenga la siguiente estructura y define los valores necesarios:
   ```json
   {
     "ConnectionStrings": {
       "ConexionString": "TU_CADENA_DE_CONEXION_A_LA_BASE_DE_DATOS_AQUI"
     },
     "JwtOptions": {
       "SecretKey": "TU_CLAVE_SECRETA_DE_AL_MENOS_32_BYTES_AQUI",
       "TokenExpirationInMinutes": 30
     }
     // Puedes a�adir otras configuraciones sensibles aqu�
   }
   ```
1. Definici�n de las variables:

   - `JwtOptions:SecretKey`:

     - **Prop�sito**: Clave utilizada para la firma digital de JSON Web Tokens (JWTs). Es crucial para la seguridad de la autenticaci�n.
     - **Requisito**: Debe ser una cadena criptogr�ficamente fuerte. Se recomienda una entrop�a de al menos 32 bytes.
     - **Generaci�n (Recomendado)**: Puedes generar una clave adecuada usando OpenSSL en tu terminal:
       ```bash
       openssl rand -base64 32
       ```
       Copia la salida de este comando y p�gala como valor para `SecretKey`.
     - **�Importante!**: Esta clave debe mantenerse secreta. La generada aqu� es para desarrollo local. Producci�n y otros entornos deben tener sus propias claves �nicas y seguras gestionadas adecuadamente.

   - `JwtOptions:TokenExpirationInMinutes`:

     - **Prop�sito**: Define la duraci�n en minutos antes de que expire un token JWT.
     - **Valor por defecto**: 30 minutos es un valor com�n para desarrollo.
     - **Consideraci�n**: Para producci�n, eval�a si este tiempo es adecuado seg�n los requisitos de seguridad y experiencia de usuario de tu aplicaci�n.

   - `ConnectionStrings:ConexionString`:
     - **Prop�sito**: Cadena de conexi�n para la base de datos principal de la aplicaci�n.
     - **Ejemplos**:
       - SQL Server LocalDB (t�pico en instalaciones de Visual Studio):
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
         Host=localhost;Port=5432;Database=NombreTuBaseDeDatos_Dev;Username=tu_usuario;Password=tu_contrase�a
         ```

#### Nota sobre otros entornos

Para entornos como Staging o Producci�n, no se utilizan los `secrets.json`. En su lugar, la configuraci�n se gestiona a trav�s de variables de entorno del sistema operativo, servicios de configuraci�n en la nube (como Azure App Configuration, AWS Systems Manager Parameter Store) o b�vedas de secretos (como HashiCorp Vault o Azure Key Vault).

### Inicializaci�n de la base de datos

Este proyecto utiliza Entity Framework Core para la gesti�n de la base de datos. Las migraciones definen el esquema de la base de datos.

1. Abrir la Consola del Administrador de Paquetes en Visual Studio:
   - ve a `Ver -> Otras Ventanas -> Consola del Administrador de Paquetes`.
1. Configurar Proyectos en la Consola:
   - Aseg�rate de que el "Proyecto predeterminado" en la Consola del Administrador de Paquetes est� configurado como `repository` (o el proyecto que contiene tus `DbContext` y Migraciones).
1. Ejecutar Migraciones:
   - Ejecuta el siguiente comando para aplicar las migraciones pendientes y crear/actualizar el esquema de tu base de datos:
     ```Powershell
     Update-Database
     ```
1. Soluci�n de Problemas Comunes:
   - `Error Unable to create an object of type 'YourDbContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728` o `Not found repository.dll` (o similar):
     - Las herramientas de Entity Framework Core a veces necesitan un proyecto de inicio (startup project) que pueda configurar y proporcionar los servicios necesarios (como el `DbContext` y la cadena de conexi�n).
     - **Soluci�n**: En Visual Studio, haz clic derecho en el `proyecto asp_services` (o tu proyecto API/Web principal) en el Explorador de Soluciones y selecciona "Establecer como proyecto de inicio". Luego, intenta ejecutar `Update-Database` nuevamente.
     - **Alternativa desde CLI (dotnet-ef)**: Si prefieres la l�nea de comandos, puedes ejecutar (desde la ra�z del repositorio o la carpeta de soluci�n):
       ```Powershell
       dotnet ef database update --project "ruta/al/proyecto/repository" --startup-project "ruta/al/proyecto/asp_services"
       ```

### Ejecutar la Aplicaci�n

Una vez configurado el entorno y la base de datos, puedes iniciar la aplicaci�n.

#### Desde Terminal (CLI)

1. Abre una terminal o S�mbolo del sistema.
1. Navega a la carpeta ra�z del proyecto `asp_services` (o tu proyecto API/Web principal).
   ```Powershell
   cd ruta/a/tu/proyecto/asp_services
   ```
1. Ejecuta el siguiente comando para compilar e iniciar la aplicaci�n:
   ```Powershell
   dotnet run
   ```
1. La aplicaci�n generalmente estar� disponible en `http://localhost:5299` y/o `https://localhost:7241` (o los puertos configurados en `Properties/launchSettings.json` del `proyecto asp_services`). Revisa la salida de la consola para ver las URLs exactas.

#### Desde Visual Studio

1. Establecer Proyecto de Inicio:
   - En el Explorador de Soluciones, haz clic derecho en el proyecto `asp_services` (o tu proyecto API/Web principal).
   - Selecciona "Establecer como proyecto de inicio".
1. Seleccionar Perfil de Inicio:
   - En la barra de herramientas superior, junto al bot�n de inicio (flecha verde), ver�s un men� desplegable con perfiles de inicio (ej: `http`, `https`, `IIS Express`). Selecciona el perfil deseado (generalmente `http` o `https` para desarrollo). Estos perfiles est�n definidos en `Properties/launchSettings.json`.
1. Ejecutar:
   - Presiona el bot�n de inicio (flecha verde) o `F5`.
1. Visual Studio abrir� autom�ticamente un navegador en la URL configurada para el perfil seleccionado.

<h2 id="frontend-configuration">Configuraci�n del Frontend</h2>

El frontend de la aplicaci�n est� desarrollado con React, TypeScript y Vite, ubicado en la carpeta `web_presentation/Spa`.

### Prerequisitos del Frontend

- Node.js v18.0 o superior
- npm (incluido con Node.js)

### Configuraci�n de Variables de Entorno del Frontend

1. **Crear archivo de configuraci�n**:

   - Navega a la carpeta `web_presentation/Spa`.
   - Encontrar�s un archivo llamado `.env.rename` que contiene las variables de entorno necesarias.
   - Crea una copia de este archivo y ren�mbralo a `.env`:
     ```Powershell
     cd web_presentation\Spa
     Copy-Item ".env.rename" ".env"
     ```

1. **Configurar las variables de entorno**:
   Edita el archivo `.env` y define los valores necesarios:

   ```bash
   # URL base de la API del backend
   VITE_API_URL=http://localhost:5299

   # Configuraci�n de Firebase (para autenticaci�n y almacenamiento)
   VITE_FIREBASE_FIREBASE_API_KEY=tu_firebase_api_key
   VITE_FIREBASE_AUTH_DOMAIN=tu_proyecto.firebaseapp.com
   VITE_FIREBASE_PROJECT_ID=tu_proyecto_id
   VITE_FIREBASE_STORAGE_BUCKET=tu_proyecto.appspot.com
   VITE_FIREBASE_MESSAGING_SENDER_ID=tu_sender_id
   VITE_FIREBASE_APP_ID=tu_app_id
   VITE_FIREBASE_MEASUREMENT_ID=tu_measurement_id
   VITE_FIREBASE_BUCKET_MULTIMEDIA=tu_bucket_multimedia
   ```

1. **Definici�n de las variables**:
   - `VITE_API_URL`: URL base donde se ejecuta tu API backend. Para desarrollo local, generalmente es `http://localhost:5299` o `https://localhost:7241`.
   - Variables de Firebase: Configuraci�n necesaria para la integraci�n con Firebase (autenticaci�n, almacenamiento de archivos, etc.). Obt�n estos valores desde tu consola de Firebase.

### Instalaci�n de Dependencias

1. **Instalar paquetes npm**:
   ```Powershell
   cd web_presentation\Spa
   npm install --legacy-peer-deps # Debido a posibles conflictos de dependencias con React 19.0
   ```

### Ejecutar el Frontend

#### Modo Desarrollo

1. Al iniciar el proyecto, aseg�rate de que el backend est� en ejecuci�n.
2. El frontend se ejecuta en paralelo con la aplicación de Razor Pages. Entonces, no es necesario iniciar un servidor separado.
3. Para iniciar la aplicación Razor Pages, abre una terminal en la carpeta `web_presentation` y ejecuta:

```Powershell
   dotnet run
```

5. Abre tu navegador y navega a `http://localhost:6299` o `https://localhost:7241` (seg��n la configuración de tu proyecto).
6. Al compilar e iniciar el proyecto, Vite se encargar�� de servir el frontend y manejar la recarga en caliente (HMR).

### Estructura del Proyecto Frontend

```
/web_presentation/
├── package.json           # Dependencias y scripts de npm
|-- package-lock.json      # Bloqueo de versiones de dependencias
├── vite.config.ts         # Configuraci�n de Vite
├── tsconfig.json          # Configuraci�n de TypeScript
└── tailwind.config.js     # Configuraci�n de Tailwind CSS
	Spa/
	├── src/                    # C�digo fuente de la aplicaci�n
	├── public/                 # Archivos est�ticos p�blicos
	├── .env                   # Variables de entorno (no incluir en git)
	├── .env.rename            # Plantilla de variables de entorno
```

### Notas Importantes

- **Seguridad**: Nunca incluyas el archivo `.env` en el control de versiones. Est� incluido en `.gitignore`.
- **Sincronizaci�n con Backend**: Aseg�rate de que la `VITE_API_URL` coincida con la URL donde se ejecuta tu backend.
- **Firebase**: Si no planeas usar Firebase, puedes dejar las variables de Firebase vac�as, pero es recomendable configurarlas para el funcionamiento completo de la aplicaci�n.
