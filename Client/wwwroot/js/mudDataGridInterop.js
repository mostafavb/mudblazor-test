window.mudDataGridInterop = {

    setFocusOnCell: (action) => {
        
        const activeElement = document.activeElement;
        if (action == 'right') {
            var isCursorAtEnd = activeElement.selectionStart === activeElement.selectionEnd && activeElement.selectionEnd === activeElement.value.length;
            if (!isCursorAtEnd)
                return;
        }
        if (action == 'left') {
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

        switch (action) {
            case 'up':
                if (rowIndex > 0)
                    rowIndex--;
                break;
            case 'down':
                if (rowIndex < rows.length)
                    rowIndex++;
                break;
            case 'left':
                if (columnIndex > 0)
                    columnIndex--;
                break;
            case 'right':
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
}