using Machine.Fakes;
using Machine.Specifications;
using Main;
using System;

namespace LruCacheSpecs
{
    public class LruCacheSpecs
    {
        class when_i_specify_an_invalid_capacity : WithSubject<LruCache<string, int>>
        {
            static Exception capacityException;

            Establish that = () => Subject = new LruCache<string, int>(1);

            Because of = () => capacityException = Catch.Exception(() => Subject = new LruCache<string, int>(0));

            It should_throw_an_exception = () => capacityException.ShouldBeAssignableTo<LruCacheException>();
        }

        class when_i_add_a_value : WithSubject<LruCache<string, int>>
        {
            Establish that = () => Subject = new LruCache<string, int>(1);

            Because of = () => Subject.AddOrUpdate("steve", 100);

            It should_have_a_count_of_one = () => Subject.Count.ShouldEqual(1);

            It should_contain_the_item = () =>
            {
                Subject.Find("steve", out int value);
                value.ShouldEqual(100);
            };
        }

        class when_i_add_a_value_and_delete_it : WithSubject<LruCache<string, int>>
        {
            Establish that = () => Subject = new LruCache<string, int>(1);

            Because of = () =>
            {
                Subject.AddOrUpdate("steve", 100);
                Subject.Remove("steve");
            };

            It should_have_a_count_of_zero = () => Subject.Count.ShouldEqual(0);

            It should_not_contain_the_item = () => Subject.IsEmpty.ShouldBeTrue();
        }

        class when_i_add_a_value_and_exceed_capacity : WithSubject<LruCache<string, int>>
        {
            Establish that = () => Subject = new LruCache<string, int>(1);

            Because of = () =>
            {
                Subject.AddOrUpdate("steve", 100);
                Subject.AddOrUpdate("mike", 101);
            };

            It should_evict_the_first_item = () => Subject.Find("steve", out int _).ShouldBeFalse();

            It should_contain_the_second_item = () =>
            {
                Subject.Find("mike", out var value);
                value.ShouldEqual(101);
            };
        }

        class when_i_add_a_value_and_update_it : WithSubject<LruCache<string, int>>
        {
            Establish that = () => Subject = new LruCache<string, int>(1);

            Because of = () =>
            {
                Subject.AddOrUpdate("steve", 100);
                Subject.AddOrUpdate("steve", 101);
            };

            It should_contain_the_updated_value = () =>
            {
                Subject.Find("steve", out var value);
                value.ShouldEqual(101);
            };
        }
    }
}
