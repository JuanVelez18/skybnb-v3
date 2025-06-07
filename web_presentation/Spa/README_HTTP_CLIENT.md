# HTTP Client con Token Refresh Automático

Este documento explica cómo usar el cliente HTTP personalizado que maneja automáticamente la renovación de tokens JWT en la aplicación Skybnb.

## Características Principales

✅ **Token Refresh Automático**: Renueva automáticamente los tokens expirados  
✅ **Cola de Solicitudes**: Gestiona múltiples solicitudes durante la renovación  
✅ **Manejo de Errores**: Manejo robusto de errores con tipos TypeScript  
✅ **Interceptores**: Inyección automática de tokens en las solicitudes  
✅ **Solicitudes Públicas**: Soporte para endpoints que no requieren autenticación  
✅ **Type Safety**: Interfaces TypeScript para respuestas y errores

## Estructura del Cliente

```
src/
├── core/
│   └── httpClient.ts          # Cliente HTTP principal
├── services/
│   ├── authService.ts         # Servicio de autenticación
│   └── propertyService.ts     # Servicio de propiedades (ejemplo)
├── components/
│   └── UserDashboard.tsx      # Componente de ejemplo
└── utils/
    └── auth.ts                # Utilidades de autenticación
```

## Uso Básico

### 1. Importar el Cliente

```typescript
import httpClient from "../core/httpClient";
import type { ApiResponse, ApiError } from "../core/httpClient";
```

### 2. Solicitudes Autenticadas

Para endpoints que requieren autenticación, usa los métodos estándar. El token se añade automáticamente:

```typescript
// GET request
const response = await httpClient.get<User>("/auth/profile");

// POST request
const response = await httpClient.post<Property>("/properties", propertyData);

// PUT request
const response = await httpClient.put<User>("/auth/profile", userData);

// DELETE request
const response = await httpClient.delete<void>(`/properties/${id}`);
```

### 3. Solicitudes Públicas

Para endpoints que NO requieren autenticación (como login/register):

```typescript
// Login (público)
const response = await httpClient.publicRequest<LoginResponse>(
  "POST",
  "/auth/login",
  { email, password }
);

// Obtener propiedades (público)
const response = await httpClient.publicRequest<Property[]>(
  "GET",
  "/properties"
);
```

## Cómo Funciona el Token Refresh

### Flujo Automático

1. **Solicitud Normal**: El cliente hace una solicitud con el token actual
2. **Token Expirado**: Si el servidor responde con 401, el cliente detecta que el token ha expirado
3. **Renovación**: El cliente pausa todas las solicitudes y renueva el token usando el refresh token
4. **Cola de Solicitudes**: Las solicitudes que fallaron se agregan a una cola
5. **Reintento**: Una vez renovado el token, se procesan todas las solicitudes en cola
6. **Continuidad**: La aplicación continúa funcionando sin interrupciones

### Ejemplo Práctico

```typescript
// Esta función puede hacer múltiples solicitudes
async function loadUserData() {
  try {
    // Si el token expira en cualquiera de estas solicitudes,
    // se renovará automáticamente y todas se completarán exitosamente
    const userResponse = await AuthService.getProfile(); // Solicitud 1
    const propertiesResponse = await PropertyService.getMyProperties(); // Solicitud 2
    const bookingsResponse = await BookingService.getMyBookings(); // Solicitud 3

    // Todas las solicitudes se completan, incluso si el token expiró
    console.log("Datos cargados exitosamente");
  } catch (error) {
    // Solo llegarás aquí si hay un error real (no relacionado con tokens)
    console.error("Error real:", error);
  }
}
```

## Servicios de Ejemplo

### AuthService

```typescript
import AuthService from "../services/authService";

// Login
const response = await AuthService.login({ email, password });

// Obtener perfil (autenticado - con token refresh)
const profile = await AuthService.getProfile();

// Logout
await AuthService.logout();
```

### PropertyService

```typescript
import PropertyService from "../services/propertyService";

// Obtener propiedades (público)
const properties = await PropertyService.getProperties(filters);

// Crear propiedad (autenticado - con token refresh)
const newProperty = await PropertyService.createProperty(propertyData);

// Obtener mis propiedades (autenticado - con token refresh)
const myProperties = await PropertyService.getMyProperties();
```

## Manejo de Errores

### Tipos de Error

```typescript
interface ApiError {
  message: string;
  status?: number;
  details?: unknown;
}
```

### Ejemplo de Manejo

```typescript
try {
  const response = await httpClient.get<User>("/auth/profile");
  console.log("Usuario:", response.data);
} catch (error) {
  const apiError = error as ApiError;

  if (apiError.status === 404) {
    console.log("Usuario no encontrado");
  } else if (apiError.status === 403) {
    console.log("Sin permisos");
  } else {
    console.log("Error:", apiError.message);
  }
}
```

## Configuración Avanzada

### Cambiar URL Base

```typescript
httpClient.setBaseURL("https://api.ejemplo.com");
```

### Headers Globales

```typescript
// Agregar header global
httpClient.setGlobalHeader("X-Client-Version", "1.0.0");

// Eliminar header global
httpClient.removeGlobalHeader("X-Client-Version");
```

### Acceso a Axios Nativo

```typescript
// Para casos especiales donde necesites acceso directo a axios
const axiosInstance = httpClient.getAxiosInstance();
```

## Casos de Uso Comunes

### 1. Dashboard con Múltiples Solicitudes

```typescript
async function loadDashboard() {
  // Todas estas solicitudes se benefician del token refresh automático
  const [userResponse, propertiesResponse, bookingsResponse] =
    await Promise.all([
      AuthService.getProfile(),
      PropertyService.getMyProperties(),
      BookingService.getMyBookings(),
    ]);

  return {
    user: userResponse.data,
    properties: propertiesResponse.data,
    bookings: bookingsResponse.data,
  };
}
```

### 2. Subida de Archivos

```typescript
async function uploadPropertyImages(propertyId: string, files: File[]) {
  // El token se incluye automáticamente, incluso para multipart/form-data
  return PropertyService.uploadPropertyImages(propertyId, files);
}
```

### 3. Solicitudes Concurrentes

```typescript
async function performMultipleActions() {
  // Si el token expira, todas estas solicitudes esperarán a la renovación
  // y luego se ejecutarán con el nuevo token
  await Promise.all([
    httpClient.put("/properties/1", { title: "Nuevo título" }),
    httpClient.post("/bookings", bookingData),
    httpClient.get("/notifications"),
  ]);
}
```

## Ventajas del Sistema

1. **Transparencia**: Los desarrolladores no necesitan pensar en la renovación de tokens
2. **Eficiencia**: Las solicitudes se agrupan durante la renovación
3. **Robustez**: Manejo automático de errores y reintentos
4. **Flexibilidad**: Soporte tanto para solicitudes públicas como autenticadas
5. **Type Safety**: Interfaces TypeScript para mejor experiencia de desarrollo

## Notas Importantes

- El refresh token se almacena de forma segura y se envía solo al endpoint de renovación
- Si el refresh token también ha expirado, el usuario será desconectado automáticamente
- Las solicitudes públicas nunca incluyen tokens de autorización
- El sistema maneja automáticamente la cola de solicitudes durante la renovación
- Todos los errores HTTP son convertidos a objetos `ApiError` tipados

## Troubleshooting

### Problema: "Unauthorized" incluso con token válido

**Solución**: Verifica que el servidor esté configurado para aceptar el header `Authorization: Bearer <token>`

### Problema: Bucle infinito de renovación

**Solución**: Asegúrate de que el endpoint `/auth/refresh` no esté protegido por autenticación

### Problema: Pérdida de contexto en solicitudes

**Solución**: Usa el sistema de cola incorporado, todas las solicitudes fallidas se reintentarán automáticamente
