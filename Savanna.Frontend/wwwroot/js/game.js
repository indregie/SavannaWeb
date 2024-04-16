let intervalId = null;

const updateBoard = async () => {

    const response = await fetch(getGameBoardUrl);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log("responseData", data);
    const boardBody = document.getElementById('boardBody');
    boardBody.innerHTML = '';
    data.forEach((row, i) => {
        const tr = document.createElement('tr');;
        row.forEach((cell, j) => {
            const td = document.createElement('td');
            td.textContent = cell;
            tr.appendChild(td);
            console.log(cell);
        });
        boardBody.appendChild(tr);
    });
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
    } catch (error) {
        console.error(error);
        alert('Failed to load game. Please try again later.');
    }
});

intervalId = setInterval(updateBoard, 1000);
