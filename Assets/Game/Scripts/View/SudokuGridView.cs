using System;
using UnityEngine;
using UnityEngine.UI;

public class SudokuGridView : MonoBehaviour
{
    [SerializeField] private SudokuCellView _cellPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private GridLayoutGroup _gridLayout;

    [SerializeField] private float _thinLine = 4f;
    [SerializeField] private float _mediumLine = 7f;
    [SerializeField] private float _thickLine =10f;

    private SudokuCellView[,] _cells;

    public void CreateGrid(int size)
    {
        ClearGrid();
        ConfigureGrid(size);

        _cells = new SudokuCellView[size, size];

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var cell = Instantiate(_cellPrefab, _container);
                _cells[r, c] = cell;
            }
        }

        SetupBorders(size);
    }

    private void ConfigureGrid(int size)
    {
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayout.constraintCount = size;

        float width = _container.rect.width;
        float height = _container.rect.height;

        float spacingX = _gridLayout.spacing.x;
        float spacingY = _gridLayout.spacing.y;

        float cellWidth = (width - spacingX * (size - 1)) / size;
        float cellHeight = (height - spacingY * (size - 1)) / size;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        _gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }

    private void ClearGrid()
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }
    }

    public void Draw(SudokuBoard board, int selectedIndex)
    {
        int size = board.Size;
        int index = 0;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var cell = board.GetCell(r, c);

                _cells[r, c].Initialize(index, cell);

                _cells[r, c].Initialize(index, cell);

                bool isSelected = index == selectedIndex;
                bool isHighlighted = selectedIndex != -1 && ComputeHighlighting(index, selectedIndex);

                _cells[r, c].SetSelected(isSelected, isHighlighted);

                index++;
            }
        }
    }

    public void SetSelected(int index, bool selected)
    {
        DrawCurrentSelection(index);
    }

    private void DrawCurrentSelection(int selectedIndex)
    {
        int size = _cells.GetLength(0);

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                int index = r * size + c;

                bool isSelected = index == selectedIndex;
                bool isHighlighted = selectedIndex != -1 && ComputeHighlighting(index, selectedIndex);

                _cells[r, c].SetSelected(isSelected, isHighlighted);
            }
        }
    }

    private bool ComputeHighlighting(int index, int selectedIndex)
    {
        int size = _cells.GetLength(0);

        int row = index / size;
        int col = index % size;

        int selectedRow = selectedIndex / size;
        int selectedCol = selectedIndex % size;

        if (row == selectedRow)
            return true;

        if (col == selectedCol)
            return true;

        int boxSize = (int)Mathf.Sqrt(size);

        int boxRow = row / boxSize;
        int boxCol = col / boxSize;

        int selectedBoxRow = selectedRow / boxSize;
        int selectedBoxCol = selectedCol / boxSize;

        if (boxRow == selectedBoxRow && boxCol == selectedBoxCol)
            return true;

        return false;
    }

    private void SetupBorders(int size)
    {
        double sqrt = System.Math.Sqrt(size);

        if (sqrt % 1 != 0)
            throw new System.ArgumentException("size must be a perfect square");

        int boxSize = (int)sqrt;

        for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    float top = GetHorizontalBorder(r, size, boxSize);
                    float bottom = GetHorizontalBorder(r + 1, size, boxSize);

                    float left = GetVerticalBorder(c, size, boxSize);
                    float right = GetVerticalBorder(c + 1, size, boxSize);

                    _cells[r, c].SetBorders(top, bottom, left, right);
                }
            }
    }

    private float GetHorizontalBorder(int row, int size, int boxSize)
    {
        if (row == 0 || row == size)
            return _thickLine;

        if (row % boxSize == 0)
            return _mediumLine;

        return _thinLine;
    }

    private float GetVerticalBorder(int col, int size, int boxSize)
    {
        if (col == 0 || col == size)
            return _thickLine;

        if (col % boxSize == 0)
            return _mediumLine;

        return _thinLine;
    }
}