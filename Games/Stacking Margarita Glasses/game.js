const gameContainer = document.getElementById('game-container');

let currentY = 0;

gameContainer.addEventListener('click', (event) => {
    const offsetX = event.clientX - gameContainer.getBoundingClientRect().left;

    const margaritaGlass = document.createElement('div');
    margaritaGlass.classList.add('margarita-glass');
    margaritaGlass.style.left = `${offsetX - 30}px`;
    margaritaGlass.style.bottom = `${currentY}px`;
    
    gameContainer.appendChild(margaritaGlass);

    currentY += 60;

    if (currentY >= gameContainer.clientHeight) {
        alert('Game over! You filled the screen with margarita glasses!');
        location.reload();
    }
});
