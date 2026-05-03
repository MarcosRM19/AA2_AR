# AA2 Gestures - Diegetic Gesture System

## Descripción
Sistema de detección de gestos con hand tracking para Meta Quest.
Permite crear gestos personalizados y vincularlos a eventos en Unity.

## Componentes

### GestureData (ScriptableObject)
Guarda los valores de curl de cada dedo y un UnityEvent.

### GestureReader (Manager)
Singleton que lee las manos y detecta gestos registrados.
Colocar en un objeto vacío en la escena.

### GestureEvent (clase base abstracta)
Heredar de esta clase para crear comportamientos vinculados a gestos.
El registro en el GestureReader es automático.

### GestureDebugger
Muestra los valores de curl de ambas manos en un Canvas World Space.
Solo para debug.

## Setup

### 1. Escena
- Añade un objeto vacío con GestureReader
- Asegúrate de tener Hand Tracking Subsystem activado en
  Project Settings → XR Plug-in Management → OpenXR → Features

### 3. Crear el GestureData
- Clic derecho en Project → AA2 Gestures → Gesture
- Añade un valor de 0-1 en cada valor de dedo (recomendable usar el debuger para valores)
- Ajusta el Threshold (por defecto 0.2)

### 4. Vincular a un comportamiento
Por código:
  - Hereda de GestureEvent
  - Implementa OnGestureTriggered()
  - Asigna el GestureData en el Inspector

### Funcionamiento
El sistema detecta con el uso de handTracking las dos manos, toma como prioridad los gestos detectados con la mano derecha, al realizarse uno de los diferentes gestos se llama al evento vinculado a ese gesto.
Ahora mismo para probar hay únicamente 2 eventos, uno que al ser detectado muestra en pantalla un texto de detección y un segundo evento que al ser detectado muestra en pantalla un texto de detección
y que esta preparado para spawnear. El sistema no esta preparado para usar manos y controladores asi que si durante la ejecución de la demo se usan los controladores, el handtracking deja de funcionar
hasta que se dejan de usar los mandos.
Los gestos preparados para la demo son: Puño Cerrado, Signo de Peace, Palma abierta, OK, Pulgar Arriba y la pose de Nappa de dragon Ball. Por si hay alguna duda de como es la pose, hay una carpeta con imagenes 
de como son estas poses


