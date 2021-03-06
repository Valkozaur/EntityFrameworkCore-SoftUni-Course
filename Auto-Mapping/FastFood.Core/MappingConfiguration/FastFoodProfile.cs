﻿namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using System;
    using System.Globalization;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Category
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name,
                y => y.MapFrom(x => x.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>()
                .ForMember(x => x.Name,
                y => y.MapFrom(x => x.Name));


            //Employee
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionId,
                y => y.MapFrom(x => x.Id))
                .ForMember(x => x.PositionName,
                y => y.MapFrom(x => x.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position,
                y => y.MapFrom(x => x.Position.Name));

            //Item
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId,
                y => y.MapFrom(x => x.Id))
                .ForMember(x => x.CategoryName,
                y => y.MapFrom(x => x.Name));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>();


            //Order
            this.CreateMap<CreateOrderViewModel, Order>()
                .ForMember(x => x.OrderItems,
                y => y.MapFrom(x => x.Items.Keys))
                .ForMember(x => x.EmployeeId,
                y => y.MapFrom(x => x.Employees.Keys));

            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x => x.DateTime,
                y => y.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.Type,
                y => y.MapFrom(x => OrderType.ToGo));

            this.CreateMap<CreateOrderInputModel, OrderItem>();
     

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.Employee, 
                y => y.MapFrom(x => x.Employee.Name))
                .ForMember(x => x.DateTime,
                y => y.MapFrom(x => x.DateTime.ToString("D", CultureInfo.InvariantCulture)));
        }
    }
}
