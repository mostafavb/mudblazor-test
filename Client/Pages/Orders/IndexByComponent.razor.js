const dataGrid = document.getElementById('generic-datagrid');
dataGrid.addEventListener("keydown", setFocusOnCell, false);

const numericboxs = document.querySelectorAll("#id-numeric");

numericboxs.forEach(numericbox => numericbox.addEventListener("keydown", numericKeydown, false));


function numericKeydown(e) {
    if (e.key === "ArrowUp" || e.key === "ArrowDown") {
        //console.log(e.key);
        e.preventDefault();
        e.stopImmediatePropagation();
        setFocusOnCell(e);
        //let clicked = e.target;
        //console.log(clicked);
    }

}

function setFocusOnCell(e) {

    if (!["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(e.key))
        return;

    const activeElement = document.activeElement;
    if (e.key === 'ArrowRight') {
        var isCursorAtEnd = activeElement.selectionStart === activeElement.selectionEnd && activeElement.selectionEnd === activeElement.value.length;
        if (!isCursorAtEnd)
            return;
    }
    if (e.key === 'ArrowLeft') {
        var isCursorAtStart = activeElement.selectionStart === activeElement.selectionEnd && activeElement.selectionStart === 0;
        if (!isCursorAtStart)
            return;
    }

    const tbody = activeElement.closest('tbody');

    const rows = tbody.getElementsByTagName('tr');

    let rowIndex = -1;
    let columnIndex = -1;
    let cellsLength = 0;


    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];

        const cells = row.querySelectorAll('.mud-table-cell.edit-mode-cell');
        cellsLength = cells.length;

        for (let j = 0; j < cells.length; j++) {
            const cell = cells[j];

            if (cell.contains(activeElement)) {
                rowIndex = i;
                columnIndex = j;
                break;
            }
        }
        if (rowIndex !== -1 && columnIndex !== -1) {
            break;
        }
    }

    //console.log('Row index:', rowIndex);
    //console.log('Column index:', columnIndex);

    switch (e.key) {
        case 'ArrowUp':
            if (rowIndex > 0)
                rowIndex--;
            break;
        case 'ArrowDown':
            if (rowIndex < rows.length)
                rowIndex++;
            break;
        case 'ArrowLeft':
            if (columnIndex > 0)
                columnIndex--;
            break;
        case 'ArrowRight':
            if (columnIndex < cellsLength)
                columnIndex++;
            break;

    }
    var activeRow = rows[rowIndex];

    var activeCells = activeRow.querySelectorAll('.mud-table-cell.edit-mode-cell');

    if (activeCells && activeCells.length > 0) {
        var activeCell = activeCells[columnIndex];

        if (activeCell) {

            var inputElement = activeCell.querySelector('.mud-table-cell.edit-mode-cell input');
            if (inputElement) {

                inputElement.focus();
            }
        }
    }
}
