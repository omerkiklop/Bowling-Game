using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Interfaces
{
    public interface IFrameMapper
    {
        IMapedFrame Map(IFrame frame);
    }
}
