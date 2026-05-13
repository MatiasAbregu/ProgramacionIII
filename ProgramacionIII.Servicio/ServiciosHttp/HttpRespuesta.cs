namespace ProgramacionIII.Servicio.ServiciosHttp
{
    public class HttpRespuesta<T>
    {
        public T? Respuesta { get; }
        public bool Error { get; }
        public HttpResponseMessage Mensaje { get; set; }

        public HttpRespuesta(T? Respuesta, bool Error, HttpResponseMessage Mensaje)
        {
            this.Respuesta = Respuesta;
            this.Error = Error;
            this.Mensaje = Mensaje;
        }
    }
}
