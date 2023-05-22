const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");

class Taco {
    constructor(x, y) {
        this.x = x;
        this.y = y;
        this.width = 30;
        this.height = 20;
    }

    draw() {
        ctx.fillStyle = "orange";
        ctx.fillRect(this.x, this.y, this.width, this.height);
    }
}

class Margarita {
    constructor(x, y) {
        this.x = x;
        this.y = y;
        this.width = 30;
        this.height = 60;
    }

    draw() {
        ctx.fillStyle = "lime";
        ctx.fillRect(this.x, this.y, this.width, this.height);
    }
}

const player = new Taco(canvas.width / 2, canvas.height - 100);
const margaritas = [];

let score = 0;

function spawnMargarita() {
    const x = Math.random() * (canvas.width - 30);
    const margarita = new Margarita(x, 10);
    margaritas.push(margarita);
}

function gameLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    player.draw();

    margaritas.forEach((margarita, index) => {
        margarita.y += 1;
        margarita.draw();

        if (margarita.y + margarita.height > canvas.height) {
            margaritas.splice(index, 1);
        }

        if (
            player.x < margarita.x + margarita.width &&
            player.x + player.width > margarita.x &&
            player.y < margarita.y + margarita.height &&
            player.y + player.height > margarita.y
        ) {
            margaritas.splice(index, 1);
            score++;
            console.log("Score:", score);
        }
    });

    if (Math.random() < 0.01) {
        spawnMargarita();
    }

    requestAnimationFrame(gameLoop);
}

gameLoop();

document.addEventListener("keydown", (event) => {
    if (event.key === "ArrowLeft") {
        player.x -= 20;
    } else if (event.key === "ArrowRight") {
        player.x += 20;
}
});

document.addEventListener("keyup", (event) => {
if (event.key === "ArrowLeft") {
player.x -= 0;
} else if (event.key === "ArrowRight") {
player.x += 0;
}
});
