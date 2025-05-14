# Skybnb
## Tabla de contenido
1. <a href="#description">Descripci�n</a>
1. <a href="#configuration">Configuraci�n de Entorno</a>

<h2 id="description">Descripci�n</h2>

Plaforma de alquiler a corto plazo de propiedades que permite a Anfitriones publicar propiedades y a Huespedes reservar y calificar su experiencia.

<h2 id="configuration">Configuraci�n de Entorno</h2>

### Variables de entorno
1. Click derecho en el proyecto `repository` y seleccionar "Administrar secretos de usuario" ("Manage User Secrets" en ingl�s).
1. Copiar el contenido del archivo `secrets-rename.json` y pegarlo en el archivo `secrets.json` abierto por el sistema.
1. Definir las variables necesarias:
	1. `SecretKey`: Debe ser una clave de m�nimo 32 bytes usada para la firma digital de los JWTs. Para su generaci�n se recomienda usar:
		```bash		
	    openssl rand --base64 32
		```
	1. `ConexionString`: String de conexi�n para la base de datos.
### Inicializaci�n base de datos
1. Abrir Consola del Administrador de Paquetes (Ver -> Otras Ventanas).
1. Seleccionar `repository` como Proyecto predeterminado.
1. Ejecutar migraciones:
	```bash		
	Update-Database
	```
1. En caso del error `Not found repository.dll`, configurar un Proyecto de Inicio (se recomienda `asp_services`).

### Levantar servidor

#### Terminal
1. Abrir terminal en la carpeta `asp_services`.
1. Ejecutar el siguiente comando:
	```bash		
	dotnet run
	```

#### Visual Studio
1. Configurar el proyecto `asp_services` como Proyecto de Inicio.
1. Seleccionar el perfil http/https.
1. Ejecutar el proyecto.