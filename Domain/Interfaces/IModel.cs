namespace Domain.Interfaces
{
    public interface IModel<DTO>
    {
        public DTO ToDTO();
    }

    public interface IDTOModel<Model>
    {
        public Model ToRawModel();
    }
}