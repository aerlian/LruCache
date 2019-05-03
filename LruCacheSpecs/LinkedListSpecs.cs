using Machine.Fakes;
using Machine.Specifications;
using Main;
using System;
using static Main.LinkedList<int,int>;

namespace LruCacheSpecs
{
    class LinkedListSpecs
    {
        class when_list_has_no_elements : WithSubject<LinkedList<int,int>>
        {
            static Node<int,int> created;

            Establish that = () => Subject = new LinkedList<int,int>();

            It should_be_empty = () => Subject.IsEmpty.ShouldBeTrue();
        }

        class when_i_add_a_value : WithSubject<LinkedList<int,int>>
        {
            static Node<int,int> created;

            Establish that = () => Subject = new LinkedList<int,int>();

            Because of = () => created = Subject.Add(0, 10);

            It should_immediately_follow_head = () => created.ShouldBeTheSameAs(Subject.head);

            It should_immediately_preceed_tail = () => created.ShouldBeTheSameAs(Subject.tail);
        }

        class when_i_add_a_value_and_remove_it : WithSubject<LinkedList<int,int>>
        {
            static Node<int,int> created;

            Establish that = () => Subject = new LinkedList<int,int>();

            Because of = () =>
            {
                created = Subject.Add(0, 10);
                Subject.Remove(created);
            };

            It should_be_empty = () => Subject.IsEmpty.ShouldBeTrue();
        }

        class when_i_remove_a_non_existent_value : WithSubject<LinkedList<int, int>>
        {
            static Node<int, int> created;
            static Exception exception;

            Establish that = () => Subject = new LinkedList<int, int>();

            Because of = () =>
            {
                created = Subject.Add(0, 10);
                Subject.Remove(created);
                exception = Catch.Exception(() => Subject.Remove(created));
            };

            It should_throw_exception = () => exception.ShouldBeAssignableTo<LruCacheException>();
        }
    }
}
