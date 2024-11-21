#include <Servo.h>

// Configuración de los servos de la cinta
const int SERVO_STOP = 90;   // Detener el servo (neutro)
const int SERVO_FORWARD = 0; // Velocidad máxima hacia adelante
const int SERVO_BACKWARD = 180; // Velocidad máxima hacia atrás

const int LED = 13; // Pin del LED

Servo servoCinta1;
Servo servoCinta2;

String comandoActual = "CINTA_OFF"; // Almacena el último comando recibido

bool ledEncendido = false; // Indica si el LED está encendido
unsigned long tiempoInicioLED = 0; // Marca de tiempo para el LED
const unsigned long duracionLED = 500; // Duración en milisegundos del LED encendido

unsigned long tiempoInicioServo = 0; // Marca de tiempo para los servos
unsigned long duracionServo = 0; // Duración del movimiento de los servos
bool servosEnMovimiento = false; // Indica si los servos están en movimiento

void setup() {
  Serial.begin(9600); // Inicializar comunicación serial

  // Conectar los servos a los pines correspondientes
  servoCinta1.attach(2);
  servoCinta2.attach(4);

  // LED configuración
  pinMode(LED, OUTPUT);
  digitalWrite(LED, LOW); // Apagar el LED al inicio

  // Inicializar los servos en estado detenido
  detenerCinta();

  Serial.println("Sistema listo para recibir comandos.");
}

void loop() {
  // Leer comandos desde el puerto serial
  if (Serial.available() > 0) {
    String command = Serial.readStringUntil('\n');
    command.trim();

    // Solo procesa comandos nuevos
    if (command != comandoActual) {
      comandoActual = command;
      ejecutarComando(comandoActual);
    }
  }

  // Controlar el temporizador del LED
  if (ledEncendido && millis() - tiempoInicioLED >= duracionLED) {
    digitalWrite(LED, LOW); // Apagar el LED
    ledEncendido = false;
  }

  // Controlar el temporizador de los servos
  if (servosEnMovimiento && millis() - tiempoInicioServo >= duracionServo) {
    detenerCinta();
    servosEnMovimiento = false;
  }
}

void ejecutarComando(String command) {
  if (command == "CINTA_ON") {
    moverServosCinta(5000); // Mueve los servos por 5 segundos
  } else if (command == "CINTA_REVERSA") {
    moverServosReversa(5000); // Mueve los servos en reversa por 5 segundos
  } else if (command == "Disparo") {
    activarLED();
  } else if (command == "CINTA_OFF") {
    detenerCinta();
  } else {
    Serial.println("Comando no reconocido.");
  }
}

void moverServosCinta(int tiempoMovimiento) {
  servoCinta1.write(SERVO_FORWARD);
  servoCinta2.write(SERVO_FORWARD);
  Serial.println("Cinta moviéndose hacia adelante.");
  tiempoInicioServo = millis(); // Guardar el tiempo de inicio
  duracionServo = tiempoMovimiento; // Configurar la duración
  servosEnMovimiento = true; // Indicar que los servos están en movimiento
}

void moverServosReversa(int tiempoMovimiento) {
  servoCinta1.write(SERVO_BACKWARD);
  servoCinta2.write(SERVO_BACKWARD);
  Serial.println("Cinta moviéndose hacia atrás.");
  tiempoInicioServo = millis(); // Guardar el tiempo de inicio
  duracionServo = tiempoMovimiento; // Configurar la duración
  servosEnMovimiento = true; // Indicar que los servos están en movimiento
}

void detenerCinta() {
  servoCinta1.write(SERVO_STOP);
  servoCinta2.write(SERVO_STOP);
  Serial.println("Cinta detenida.");
  servosEnMovimiento = false; // Indicar que los servos se han detenido
}

void activarLED() {
  digitalWrite(LED, HIGH); // Encender el LED
  Serial.println("LED encendido por comando DISPARO.");
  tiempoInicioLED = millis(); // Reiniciar el tiempo de inicio del LED
  ledEncendido = true; // Indicar que el LED está encendido
}
