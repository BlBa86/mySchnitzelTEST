﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCloud.Models.Domain;

namespace GCloud.Repository
{
    public interface ITagRepository : IAbstractRepository<Tag>
    {
        Tag FindByName(string tagName);
    }
}