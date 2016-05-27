namespace BLL.Services.ReportService.Abstract
{
    /// <summary>
    /// Draws text with specified style for docx or pdf
    /// </summary>
    public abstract class Designer
    {
        /// <summary>
        /// Adds text to document
        /// </summary>
        /// <param name="text">Paragraph of text</param>
        public abstract void Draw(string text);
    }
}
