# Skybnb
## Tabla de contenido
1. <a href="#description">Descripción</a>
1. <a href="#configuration">Configuración de Entorno</a>

<h2 id="description">Descripción</h2>

Plaforma de alquiler a corto plazo de propiedades que permite a Anfitriones publicar propiedades y a Huespedes reservar y calificar su experiencia.

<h2 id="configuration">Configuración de Entorno</h2>

### Variables de entorno
1. Click derecho en el proyecto `repository` y seleccionar "Administrar secretos de usuario" ("Manage User Secrets" en inglés).
1. Copiar el contenido del archivo `secrets-rename.json` y pegarlo en el archivo `secrets.json` abierto por el sistema.
1. Definir las variables necesarias:
	1. `SecretKey`: Debe ser una clave de mínimo 32 bytes usada para la firma digital de los JWTs. Para su generación se recomienda usar:
		```bash		
	    openssl rand --base64 32
		```
	1. `ConexionString`: String de conexión para la base de datos.
### Inicialización base de datos
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