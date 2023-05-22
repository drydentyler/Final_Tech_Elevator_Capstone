const car = document.getElementById('car');
const parkingSpots = document.querySelectorAll('.parkingSpot');

let currentCarPosition = 0;
let currentEmptySpot = Math.floor(Math.random() * 10);

// Movement speed
const speed = 5;

// Add event listener for keyboard controls
document.addEventListener('keydown', (event) => {
  switch (event.key) {
    case 'ArrowUp':
      car.style.top = `${parseInt(car.style.top || 0) - speed}px`;
      break;
    case 'ArrowDown':
      car.style.top = `${parseInt(car.style.top || 0) + speed}px`;
      break;
    case 'ArrowLeft':
      car.style.left = `${parseInt(car.style.left || 0) - speed}px`;
      break;
    case 'ArrowRight':
      car.style.left = `${parseInt(car.style.left || 0) + speed}px`;
      break;
  }
});

function setNewTargetSpot() {
  const previousEmptySpot = currentEmptySpot;
  while (currentEmptySpot === previousEmptySpot) {
    currentEmptySpot = Math.floor(Math.random() * 10);
  }

  car.style.backgroundColor = `#${Math.floor(Math.random() * 16777215).toString(16)}`;
}

setNewTargetSpot();

