﻿using App.Domain.Core.OrderAgg.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.OrderAgg.Contracts.IServices
{
    public interface ICommentService
    {
        public void Delete(int id);
        public void EnableShow(int id);
        public void DisableShow(int id);
        public List<CommentDto> OrderComments(int orderId);
        public CommentDto Details(int id);

    }
}
