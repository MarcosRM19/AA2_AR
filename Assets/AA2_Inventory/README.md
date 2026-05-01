# AA2 Inventory - Diegetic Inventory System

## Setup
1. Añade `BodyAnchor` como hijo de XR Origin
2. Añade `HolsterManager` a la escena
3. Añade `CatchWeapon` a tu arma
4. Configura las referencias en el Inspector

## Controles
- Mantener botón B/Y 1.5 segundos → crear holster
- Acercar arma al holster y soltar → acoplar al holster

## Funcionalidad
El componente consiste en un inventario diegético que te permite guardar armas en puntos invisible, ahora mismo se marcan estos puntos invisibles con esferas azules en el mapa para saber donde estan,
el sistema esta hecho para que presionando los botones secundarios de los mandos B/Y se cree un holster en el espacio, si este esta cerca del cuerpo del jugador se atachea a este y mientras el juagdor se mueva
este lo seguirá, siendo en ese caso el inventario del jugador, en el caso que este alejado del cuerpo se genera en el espacio y no seguirá al jugador, siendo en ese caso un inventario como una estanterioa, un cofre...
o otros casos. Dependiendo de hacia donde se apunta cuando se crea el punto la rotación del arma cuando sea acoplada cambiará.
Como escena inicial hay 5 katanas y 5 puntos de inventario que estan generados en el espacio, el jugador puede crear holsters infinitos siguiendo lo explicado anteriormente
Para acoplar una espada al holster se ha de acerca la zona de attach al holster (donde esta la mano) y al soltarla se acoplará, siempre que ese holster no esta ocupa ya por una arma