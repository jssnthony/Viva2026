Project arcdteam.viva2026
Concepto General
arcdteam.viva2026 es un juego de plataformas 2D para celular inspirado en Scott Pilgrim vs. the World: The Game y Super Mario Run, pero más rápido y frenético.
Controlas a un aficionado de fútbol que corre por las calles de la Ciudad de México para llegar al estadio antes de que termine el partido del Mundial FIFA 2026.
La mecánica principal es correr, saltar, deslizarse y patear balones de fútbol para eliminar obstáculos y enemigos que te impiden llegar a tiempo.
El objetivo es llegar a la meta (el estadio) dentro del tiempo límite, esquivando tráfico, burlando policías, esquivando manifestantes y pateando balones a todo lo que se interponga.
---
Visión
Crear un juego de acción rápido, gracioso y sencillo con controles táctiles intuitivos. Ambientado en la CDMX durante el Mundial 2026, el juego debe ser fácil de aprender pero difícil de dominar, con un ritmo frenético donde el jugador nunca se detiene por mucho tiempo.
---
Inspiraciones
Scott Pilgrim vs. the World: The Game
Super Mario Run (celular)
Katana Zero (ritmo rápido)
Hotline Miami (acción instantánea)
---
Mecánica Principal
El jugador avanza por la ciudad hacia el estadio. En el camino se encuentra con:
Tráfico (coches, microbuses) → obstáculos que dañan al contacto. Se saltan por encima.
Obstáculos bajos (vallas, barreras) → se pasan deslizándose por debajo (slide).
Policías → enemigos que patrullan y dañan al contacto. Se eliminan pateándoles un balón.
Autos en movimiento → van de derecha a izquierda, dañan al contacto, se saltan, no se eliminan.
Tiempo límite de 2 minutos en pantalla. Si llega a cero, pierdes.
---
Wishlist (ideas futuras)
Power-ups con camisetas/bufandas de equipos, balones, etc. aun no se me ocurre nada sobre esto
---
MVP (Minimum Viable Product)
Características del MVP
Controles
Botones táctiles en pantalla (◀ ▶ movimiento, SALTO, SLIDE, PATADA).
También funciona con teclado (A/D o flechas, Espacio, C, K o clic izquierdo).
Jugador
Movimiento izquierda y derecha manual con aceleración.
Salto (brincar tráfico y obstáculos).
Slide/deslizarse (pasar por debajo de obstáculos bajos, reduce collider + velocidad extra).
Patada de balón (proyectil infinito, cooldown 0.3s, elimina policías).
Sistema de vida simple (3 vidas, invulnerabilidad post-daño).
Enemigos y obstáculos
Policía: Patrulla una zona. Daña al contacto. Muere al recibir un balonazo.
Tráfico estático: Obstáculo rojo. Daña al contacto. Se brinca.
Auto en movimiento: Va de derecha a izquierda. Daña al contacto. Se brinca. No se elimina.
Valla/barrera baja: Naranja con rayas. Daña si pasas de pie. Se pasa deslizándose sin daño.
Nivel (MVP actual)
Un solo nivel de ~80 unidades de ancho, generado proceduralmente.
5 secciones: Tutorial → Tráfico → Policías → Mixto → Final.
Punto de inicio (x=-7) y meta (x=80, rectángulo verde).
Timer de 2 minutos visible en pantalla (se pone rojo a los 30s).
Victoria
Llegar al rectángulo verde (entrada del estadio).
Derrota
Perder toda la vida o que el tiempo llegue a cero.
Presiona R o toca la pantalla para reiniciar.
---
Leyenda de colores (sprites temporales)
Verde oscuro (#006847) = Jugador (aficionado de fútbol)
Gris oscuro = Suelo/calles
Gris medio = Plataformas/banquetas
Rojo = Tráfico estático (coches estacionados, microbuses)
Rojo con ventanas = Auto en movimiento (va de derecha a izquierda)
Azul marino = Policía (con cara piel)
Naranja con rayas = Valla/barrera baja (requiere slide)
Verde brillante = Meta (entrada del estadio)
Blanco con borde negro = Balón (proyectil de la patada)
Azul cielo = Fondo del nivel (cielo CDMX)
Gris claro (botones UI) = Controles táctiles (semi-transparentes)
