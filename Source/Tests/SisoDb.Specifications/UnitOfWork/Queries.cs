﻿using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using PineCone.Structures;
using SisoDb.Resources;
using SisoDb.Specifications.Model;
using SisoDb.Testing;

namespace SisoDb.Specifications.UnitOfWork
{
    namespace Count
    {
        [Subject(typeof(IUnitOfWork), "Count")]
        public class when_counting_using_expression_matching_two_of_four_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () => TestContext = TestContextFactory.Create();

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(QueryGuidItem.CreateFourItems<QueryGuidItem>());

                    _count = uow.Count<QueryGuidItem>(x => x.SortOrder >= 3);
                }
            };

            It should_result_in_count_of_2 = () => _count.ShouldEqual(2);

            private static int _count;
        }
    }

    namespace GetAll
    {
        [Subject(typeof(IUnitOfWork), "Get all")]
        public class when_set_with_guid_contains_four_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.GetAll<QueryGuidItem>().ToList();
                }
            };

            It should_fetch_all_4_structures =
                () => _fetchedStructures.Count.ShouldEqual(4);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldBeValueEqualTo(_structures[0]);
                _fetchedStructures[1].ShouldBeValueEqualTo(_structures[1]);
                _fetchedStructures[2].ShouldBeValueEqualTo(_structures[2]);
                _fetchedStructures[3].ShouldBeValueEqualTo(_structures[3]);
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<QueryGuidItem> _fetchedStructures;
        }

        [Subject(typeof(IUnitOfWork), "Get all as Json")]
        public class when_set_with_guid_contains_four_json_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.GetAllAsJson<QueryGuidItem>().ToList();
                }
            };

            It should_fetch_all_4_structures =
                () => _fetchedStructures.Count.ShouldEqual(4);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldBeValueEqualTo(_structures[0].AsJson());
                _fetchedStructures[1].ShouldBeValueEqualTo(_structures[1].AsJson());
                _fetchedStructures[2].ShouldBeValueEqualTo(_structures[2].AsJson());
                _fetchedStructures[3].ShouldBeValueEqualTo(_structures[3].AsJson());
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<string> _fetchedStructures;
        }
    }

    namespace GetById
    {
        [Subject(typeof(IUnitOfWork), "Get by Id (guid)")]
        public class when_set_with_guid_contains_four_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = TestContext.Database.WriteOnce().InsertMany(QueryGuidItem.CreateFourItems<QueryGuidItem>());
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructure = uow.GetById<QueryGuidItem>(_structures[1].StructureId);
                }
            };

            It should_fetch_the_structure =
                () => _fetchedStructure.ShouldBeValueEqualTo(_structures[1]);

            private static IList<QueryGuidItem> _structures;
            private static QueryGuidItem _fetchedStructure;
        }
    }

    namespace GetByIdInterval
    {
        [Subject(typeof(IUnitOfWork), "Get by Id interval")]
        public class when_getting_for_guids : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                CaughtException = Catch.Exception(() =>
                {
                    using (var uow = TestContext.Database.CreateUnitOfWork())
                    {
                        uow.InsertMany(_structures);

                        _fetchedStructures = uow.GetByIdInterval<QueryGuidItem>(_structures[1].StructureId, _structures[2].StructureId).ToList();
                    }
                });
            };

            It should_have_failed = () =>
            {
                CaughtException.ShouldNotBeNull();
                CaughtException.ShouldBeOfType<SisoDbNotSupportedByProviderException>();

                var ex = (SisoDbNotSupportedByProviderException)CaughtException;
                ex.Message.ShouldContain(ExceptionMessages.QueryEngine_GetByIdInterval_WrongIdType);
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<QueryGuidItem> _fetchedStructures;
        }
    }

    namespace GetByIds
    {
        [Subject(typeof(IUnitOfWork), "Get by Ids")]
        public class when_ids_matches_two_of_four_items_that_is_are_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.GetByIds<QueryGuidItem>(_structures[1].StructureId, _structures[2].StructureId).ToList();
                }
            };

            It should_fetch_2_structures =
                () => _fetchedStructures.Count.ShouldEqual(2);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldBeValueEqualTo(_structures[1]);
                _fetchedStructures[1].ShouldBeValueEqualTo(_structures[2]);
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<QueryGuidItem> _fetchedStructures;
        }

        [Subject(typeof(IUnitOfWork), "Get by Ids as Json")]
        public class when_ids_matches_two_of_four_json_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.GetByIdsAsJson<QueryGuidItem>(_structures[1].StructureId, _structures[2].StructureId).ToList();
                }
            };

            It should_fetch_2_structures =
                () => _fetchedStructures.Count.ShouldEqual(2);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldEqual(_structures[1].AsJson());
                _fetchedStructures[1].ShouldEqual(_structures[2].AsJson());
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<string> _fetchedStructures;
        }
    }

    namespace Where
    {
        [Subject(typeof(IUnitOfWork), "Where")]
        public class when_expression_matches_two_of_four_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.Where<QueryGuidItem>(x => x.SortOrder >= _structures[1].SortOrder && x.SortOrder <= _structures[2].SortOrder).ToList();
                }
            };

            It should_fetch_2_structures =
                () => _fetchedStructures.Count.ShouldEqual(2);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldBeValueEqualTo(_structures[1]);
                _fetchedStructures[1].ShouldBeValueEqualTo(_structures[2]);
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<QueryGuidItem> _fetchedStructures;
        }
    }

    namespace Query
    {
        [Subject(typeof(IUnitOfWork), "Query")]
        public class when_query_matches_two_of_four_items_that_are_in_uncommitted_mode : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
            };

            Because of = () =>
            {
                using (var uow = TestContext.Database.CreateUnitOfWork())
                {
                    uow.InsertMany(_structures);

                    _fetchedStructures = uow.Query<QueryGuidItem>(q => q.Where(x => x.SortOrder >= _structures[1].SortOrder && x.SortOrder <= _structures[2].SortOrder)).ToList();
                }
            };

            It should_fetch_2_structures =
                () => _fetchedStructures.Count.ShouldEqual(2);

            It should_fetch_the_two_middle_structures = () =>
            {
                _fetchedStructures[0].ShouldBeValueEqualTo(_structures[1]);
                _fetchedStructures[1].ShouldBeValueEqualTo(_structures[2]);
            };

            private static IList<QueryGuidItem> _structures;
            private static IList<QueryGuidItem> _fetchedStructures;
        }
    }
}