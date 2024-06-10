using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;


namespace DBObjectsViewer
{
    public static class AppWordProps
    {
        public static Paragraph PageBreak()
        {
            return new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
        }

        public static TableCellProperties VerticalCenterCellValue()
        {
            return new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
        }

        public static ParagraphProperties HorizontalCenterCellValue()
        {
            return new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
        }

        public static TableCellProperties TableWidthOnPage()
        {
            return new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "9999" });
        }

        public static RunProperties BoldText()
        {
            return new RunProperties(new Bold());
        }

        public static StyleParagraphProperties TableHeaderSpacing()
        {
            return new StyleParagraphProperties(new SpacingBetweenLines() { Before = "200", After = "200", Line = "200", LineRule = LineSpacingRuleValues.Exact }); 
        }

        public static StyleParagraphProperties TableDataSpacing()
        {
            return new StyleParagraphProperties(new SpacingBetweenLines() { Before = "50", After = "50", Line = "200", LineRule = LineSpacingRuleValues.Exact });
        }

        public static void MergeRow(TableCell tCell)
        {
            TableCellProperties cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });

            TableCellProperties cellTwoProperties = new TableCellProperties();
            cellTwoProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Continue
            });

            tCell.Append(cellOneProperties);
            tCell.Append(cellTwoProperties);
        }

        public static TableProperties StandartTableProps()
        {
            UInt32Value tBorderSize = 1;
            return new TableProperties(
                 new TableBorders(
                     new TopBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     },
                     new BottomBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     },
                     new LeftBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     },
                     new RightBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     },
                     new InsideHorizontalBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     },
                     new InsideVerticalBorder
                     {
                         Val = new EnumValue<BorderValues>(BorderValues.Single),
                         Size = tBorderSize
                     }
                 ),
                 new TableLayout { Type = TableLayoutValues.Autofit },
                 new TableWidth { Type = TableWidthUnitValues.Auto }
                 );
        }
    }
}
