let intervalId = null;
let gameRunning = false;
let animals = {};

const updateBoard = async () => {
    const response = await fetch(getGameBoardUrl);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    gameRunning = true;
    const data = await response.json();
    console.log("responseData", data);
    const boardBody = document.getElementById('boardBody');
    boardBody.innerHTML = '';
    data.board.forEach((row, i) => {
        const tr = document.createElement('tr');;
        row.forEach((cell, j) => {
            const td = document.createElement('td');
            td.textContent = cell;
            td.classList.add('animal-cell');
            td.dataset.animalId = cell;
            td.addEventListener('mouseenter', handleMouseEnter);
            td.addEventListener('mouseleave', handleMouseLeave);
            tr.appendChild(td);
            console.log(cell);
        });
        boardBody.appendChild(tr);
    });

    const iterationCountSpan = document.getElementById('iterationCount');
    const animalCountSpan = document.getElementById('animalCount');
    iterationCountSpan.textContent = data.iterationCount;
    animalCountSpan.textContent = data.animals.length;

    for (const a of data.animals) {
        animals[a.Id] = a;
    }
}

document.getElementById('addAnimalButton').addEventListener('click', async (event) => {
    event.preventDefault();
    console.log("listener alive");
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
        console.log("response fetched");
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
            body: JSON.stringify({ /* data for saving the game */ })
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
    //call new game endpoint to clear Animals list
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
        //alert('Are you sure you want to leave? Your progress will not be saved.');
    }
});

const getAnimalStats = async (animalId) => {
    try {
        const response = await fetch(`/Game/GetAnimalStats?animalId=${animalId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch animal statistics');
        }
        return await response.json();
    } catch (error) {
        console.error(error);
        return null;
    }
};

//click on animal
const handleMouseEnter = async (event) => {
    const animalId = event.target.dataset.Id;
    const tooltip = document.getElementById('tooltip');
    //const animalStats = await getAnimalStats(animalId);
    animal = animals[animalId];
    //if (animalStats) {
    //    tooltip.textContent = `${animalStats.species}. Age: ${animalStats.age}, Health: ${animalStats.health}, Offsprings ${animalStats.offsprings}`;
    //    tooltip.style.display = 'block'; //show tooltip
    //    tooltip.style.left = `${event.pageX}px`;
    //    tooltip.style.top = `${event.pageY}px`;
    //} else {
    if (animal != null) {
        console.log('animal clicked')
        tooltip.textContent = `${animal.species}. Age: ${animal.age}, Health: ${animal.health}, Offsprings ${animal.offsprings}`;
        tooltip.style.display = 'block'; //show tooltip
        tooltip.style.left = `${event.pageX}px`;
        tooltip.style.top = `${event.pageY}px`;
    } else {
        tooltip.style.display = 'none';
    }    
}

const handleMouseLeave = () => {
    const tooltip = document.getElementById('tooltip');
    tooltip.style.display = 'none';
}
