const puzzleBoard = document.querySelector('#puzzle')
const puzzleName = document.querySelector('#guid')
const solveButton = document.querySelector('#solve-button')
const solutionText = document.querySelector('#solution')
const squares = 81
const submission = []

puzzleName.innerHTML = "Default"

for(let i = 0; i < squares; i++) {
    const inputElement = document.createElement('input')
    inputElement.setAttribute('type', 'number')
    inputElement.setAttribute('min', 1)
    inputElement.setAttribute('max', 9)
    if (
        ((i % 9 == 0 || i % 9 == 1 || i % 9 == 2) && i < 21) ||
        ((i % 9 == 6 || i % 9 == 7 || i % 9 == 8) && i < 27) ||
        ((i % 9 == 3 || i % 9 == 4 || i % 9 == 5) && (i > 27 && i < 53)) ||
        ((i % 9 == 0 || i % 9 == 1 || i % 9 == 2) && i > 53) ||
        ((i % 9 == 6 || i % 9 == 7 || i % 9 == 8) && i > 53) 
    ) {
        inputElement.classList.add('odd-section')
    }
    puzzleBoard.appendChild(inputElement)
}


const joinValues = () => {
    submission.length = 0
    const inputs = document.querySelectorAll('input')
    inputs.forEach(input => {
        if (input.value) {
            submission.push(parseInt(input.value))
        }else {
            submission.push(0)
        }
    })
    console.log(submission)
}

const populateValues = (data) => {
    if (data.solvable) {
        const inputs = document.querySelectorAll('input')
        inputs.forEach((input, i) => {
            input.value = data.board[i]
        })
        solutionText.innerHTML = `Puzzle <strong>${data.name}</strong> solved on ${data.dateTimeStamp}`
    } else {
        solutionText.innerHTML = `Puzzle not solvable`
    }
}

const solve = () => {
    joinValues()
    var options = {
        method: 'POST',
        url: 'https://sudokuopenapiexample.azurewebsites.net/api/sudoku-solver-post',
        headers: {
            'content-type': 'application/json',
        },
        data: {
            "name": puzzleName.innerHTML,
            "board": submission,
            "solvable": true,
            "dateTimeStamp": new Date()
        }
    }

    axios.request(options).then((response) => {
        console.log(response.data)
        populateValues(response.data)
    }).catch((error) => {
        console.log(error)
    })
}

solveButton.addEventListener('click', solve)
