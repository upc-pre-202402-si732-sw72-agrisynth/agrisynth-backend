﻿using agrisynth_backend.Resource.Domain.Model.Aggregates;
using agrisynth_backend.Resource.Domain.Model.Commands;

namespace agrisynth_backend.Resource.Domain.Services; 

public interface IResourceItemCommandService
{
    Task<ResourceItem?> Handle(CreateResourceItemCommand command);
    Task<ResourceItem?> Handle(DeleteResourceItemCommand command);
    Task<ResourceItem?> Handle(UpdateResourceItemCommand command); 
}