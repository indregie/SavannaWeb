let intervalId = null;
let gameRunning = false;
let animals = {};
let displayIcons = false;

const updateBoard = async () => {
    updateBoardIcons();
    return;
    if (displayIcons) {
        updateBoardIcons();
    } else {
        updateBoardLetters();
    }
}

const updateBoardLetters = async () => {

    const response = await fetch(getGameBoardUrl);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    gameRunning = true;
    const data = await response.json();

    animals = {};
    for (const animal of data.animals) {
        animals[animal.id] = animal;
    }

    const boardBody = document.getElementById('boardBody');
    boardBody.innerHTML = '';
    data.board.forEach((row) => {
        const tr = document.createElement('tr');;
        row.forEach((animalId) => {
            let animalSymbol = "-";
            if (animalId != null) {
                const animal = animals[animalId];
                animalSymbol = animal.symbol;
            }
            const td = document.createElement('td');
            td.textContent = animalSymbol;
            td.classList.add('animal-cell');
            td.dataset.animalId = animalId;
            td.addEventListener('click', animalMouseClick);
            tr.appendChild(td);
        });
        boardBody.appendChild(tr);
    });

    const iterationCountSpan = document.getElementById('iterationCount');
    const animalCountSpan = document.getElementById('animalCount');
    iterationCountSpan.textContent = data.iterationCount;
    animalCountSpan.textContent = data.animals.length;

}
const updateBoardIcons = async () => {

    const response = await fetch(getGameBoardUrl);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    gameRunning = true;
    const data = await response.json();

    animals = {};
    for (const animal of data.animals) {
        animals[animal.id] = animal;
    }

    const boardBody = document.getElementById('boardBody');
    boardBody.innerHTML = '';
    data.board.forEach((row) => {
        const tr = document.createElement('tr');;
        row.forEach((animalId) => {
            const td = document.createElement('td');

            
            if (animalId != null) {
                const animal = animals[animalId];
                const animalSymbol = animal.symbol;
                const animalIcon = document.createElement("img");
                animalIcon.src = `animalIcon/${animalSymbol}`;
                td.appendChild(animalIcon);
            } else {
                td.textContent = "-";
            }
            
            td.classList.add('animal-cell');
            td.dataset.animalId = animalId;
            td.addEventListener('click', animalMouseClick);
            tr.appendChild(td);
        });
        boardBody.appendChild(tr);
    });

    const iterationCountSpan = document.getElementById('iterationCount');
    const animalCountSpan = document.getElementById('animalCount');
    iterationCountSpan.textContent = data.iterationCount;
    animalCountSpan.textContent = data.animals.length;

}
document.getElementById('addAnimalButton').addEventListener('click', async (event) => {
    event.preventDefault();
    try {
        var animalSymbolInput = document.getElementById('animalSymbol');
        var animalSymbol = animalSymbolInput.value.toUpperCase();
        const response = await fetch(handleInputUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ animalSymbol: animalSymbol })
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        animalSymbolInput.value = '';
    } catch (error) {
        console.error(error);
    }
});

document.getElementById('saveGameButton').addEventListener('click', async (event) => {
    event.preventDefault();
    gameRunning = false;
    try {
        // stop gameboard
        clearInterval(intervalId);
        document.getElementById('gameContent').classList.add('hidden');
        const response = await fetch(saveGameUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        console.log('Game saved succesfully');
    } catch (error) {
        console.error(error);
    }
});

document.getElementById('newGameButton').addEventListener('click', async () => {
    try {
        const response = await fetch(newGameUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Failed to start new game: ' + response.statusText);
        }

        clearInterval(intervalId);
        await updateBoard();
        intervalId= setInterval(updateBoard, 1000);
        document.getElementById('gameContent').classList.remove('hidden');
    } catch (error) {
        console.error(error);
    }
});

document.getElementById('loadGameButton').addEventListener('click', async () => {
    try {
        const response = await fetch(loadGameUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (!response.ok) {
            throw new Error('Failed to load game: ' + response.statusText);
        }
        console.log('game loaded successfully');
        clearInterval(intervalId);
        await updateBoard();
        intervalId = setInterval(updateBoard, 1000);
        document.getElementById('gameContent').classList.remove('hidden');
    } catch (error) {
        console.error(error);
        alert('Failed to load game. Please try again later.');
    }
});

window.addEventListener('beforeunload', (event) => {
    if (gameRunning) {
        event.preventDefault();
        event.returnValue = '';
    }
});

const animalMouseClick = async (event) => {
    const animalId = event.target.dataset.animalId;
    const tooltip = document.getElementById('tooltip');
    animal = animals[animalId];
    if (animal != null) {
        console.log('animal clicked', animalId, tooltip);
        tooltip.textContent = `${animal.symbol}. Age: ${animal.age}, Health: ${animal.health}, Offsprings ${animal.offsprings}`;
        tooltip.style.display = 'block'; //show tooltip
        tooltip.style.left = `${event.pageX}px`;
        tooltip.style.top = `${event.pageY}px`;
    } else {
        tooltip.style.display = 'none'; 
    }    
}

document.body.addEventListener('click', (event) => {
    const tooltip = element.getElementById('tooltip');
    const isClickInsideTooltip = tooltip.contains(event.target);
    if (!isClickInsideTooltip) {
        tooltip.style.display = 'none';
    }
});
