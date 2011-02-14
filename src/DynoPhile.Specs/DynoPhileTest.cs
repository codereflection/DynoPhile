using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace DynoPhile.Specs
{
    public class importing_a_quoted_csv_file
    {
        private static DynoPhileReader dyno;
        private static string fileName;
        private static string delimitor;
        private static IList<dynamic> result;

        Establish context = () =>
                                {
                                    fileName = "TestFiles\\Twitters.csv";
                                    delimitor = ",";
                                    dyno = new DynoPhileReader();
                                };

        Because of = () =>
            result = dyno.ReadFile(fileName, delimitor);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_have_3_rows = () =>
            result.Count.ShouldEqual(3);

        It should_have_the_first_twitterer_first_name = () =>
            (result.First().FirstName as string).ShouldEqual("Jeff");

        It should_have_the_first_twitterer_last_name = () =>
            (result.First().LastName as string).ShouldEqual("Schumacher");

        It should_have_the_first_twitterer_handle = () =>
            (result.First().Twitter as string).ShouldEqual("codereflection");

    }

    public class importing_a_nonquoted_csv_file
    {
        private static DynoPhileReader dyno;
        private static string fileName;
        private static string delimitor;
        private static IList<dynamic> result;

        Establish context = () =>
        {
            fileName = "TestFiles\\Emails.csv";
            delimitor = ",";
            dyno = new DynoPhileReader();
        };

        Because of = () =>
            result = dyno.ReadFile(fileName, delimitor);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_have_3_rows = () =>
            result.Count.ShouldEqual(3);

        It should_have_the_first_first_name = () =>
            (result.First().FirstName as string).ShouldEqual("Jeff");

        It should_have_the_first_last_name = () =>
            (result.First().LastName as string).ShouldEqual("Schumacher");

        It should_have_the_first_email_address = () =>
            (result.First().Email as string).ShouldEqual("jeff@codingreflection.com");

        It should_handle_field_names_with_spaces = () =>
            (result.First().CurrentEmployer as string).ShouldEqual("Russell Investments");

    }

}
