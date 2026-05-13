namespace ProgramacionIII.Shared.DTO_Response
{
    public class Response<T>
    {
        public T? Objeto { get; set; }
        public string? Mensaje { get; set; }
        public bool Estado { get; set; }
    }
}
