namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa {  get; }            //Este vai aceder à Interface da Villa
        IVillaNumberRepository VillaNumber { get; } //Este vai aceder à Interface do Villa Number
        IAmenityRepository Amenity { get; }         //Este vai aceder à Interface da Amenity
        void Save();  
    }
}
