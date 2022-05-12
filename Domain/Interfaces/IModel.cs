namespace Domain.Interfaces
{
    public interface IModel<DTO>
    {
        public DTO ItemToDTO();
    }
}