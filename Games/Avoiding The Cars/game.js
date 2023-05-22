const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");

class Car {
    constructor(x, y, width, height, color) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.color = color;
    }

    draw() {
        ctx.fillStyle = this.color;
        ctx.fillRect(this.x, this.y, this.width, this.height);
    }

    update() {
        this.draw();
    }
}

const playerCar = new Car(375, 500, 50, 100, "blue");
const enemyCars = [];

function spawnEnemyCars() {
    setInterval(() => {
        const enemyCar = new Car(Math.random() * (canvas.width - 50), -100, 50, 100, "red");
        enemyCars.push(enemyCar);
    }, 2000);
}

function update() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    playerCar.update();
    enemyCars.forEach((car, index) => {
        car.y += 2;
        car.update();

        if (car.y > canvas.height) {
            enemyCars.splice(index, 1);
        }

        if (
            playerCar.x < car.x + car.width &&
            playerCar.x + playerCar.width > car.x &&
            playerCar.y < car.y + car.height &&
            playerCar.y + playerCar.height > car.y
        ) {
            alert("Game Over!");
            location.reload();
        }
    });
    requestAnimationFrame(update);
}

spawnEnemyCars();
update();

document.addEventListener("keydown", (event) => {
    if (event.key === "ArrowLeft" && playerCar.x > 0) {
        playerCar.x -= 10;
    } else if (event.key === "ArrowRight" && playerCar.x + playerCar.width < canvas.width) {
        playerCar.x += 10;
    }
});
