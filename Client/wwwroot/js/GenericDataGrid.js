﻿
//function activeNumericFocus() {
//    const numericboxs = document.querySelectorAll("#id-numeric");

//    numericboxs.forEach(numericbox => numericbox.addEventListener('keydown', numericKeydown, false));
//}


//function numericKeydown(e) {
//    if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
//        setFocusOnCell(e);
//    }
//}

function activeFocus() {
    const dataGrid = document.getElementById('generic-datagrid');
    if (dataGrid)
        dataGrid.addEventListener('keydown', setFocusOnCell, false);
}

function setFocusOnCell(e) {

    if (!["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(e.key))
        return;

    const activeElement = document.activeElement;
    if (activeElement.tagName === 'INPUT' || activeElement.tagName === 'TEXTAREA') {
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
    }
    e.preventDefault();
    e.stopImmediatePropagation();

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
            var nextElement = null;
            if (activeCell.querySelector('.mud-select') !== null)
                nextElement = activeCell.querySelector('.mud-select-input div');
            else
                nextElement = activeCell.querySelector('.mud-table-cell.edit-mode-cell input');
            if (nextElement) {
                nextElement.focus();
            }
        }
    }
}

//let firstCheck = true;
//export function observeAutoGeneratedDiv(dotNetObject, callback) {
//    firstCheck = true;

//    const targetNode = document;

//    const observer = new MutationObserver((mutationsList) => {
//        const dialog = document.querySelector('[id*=dialog]');
//        if (dialog) {
//            if (firstCheck) {
//                dotNetObject.invokeMethodAsync(callback, true);
//                firstCheck = false;
//            }
//            return;
//        }
//        else {

//            dotNetObject.invokeMethodAsync(callback, false);
//            if (!firstCheck)
//                observer.disconnect();
//            return;
//        }
//    });

//    observer.observe(targetNode, { childList: true, subtree: true });
//}



window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}