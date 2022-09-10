using UnityEngine;

public class test : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnAppStart()
    {
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void TestDataConverter()
    {
        /*DataConverter.Convert<int, int>(1);
        DataConverter.Convert<int, long>(1);
        DataConverter.Convert<int, float>(1);
        DataConverter.Convert<int, double>(1);
        DataConverter.Convert<int, bool>(1);
        DataConverter.Convert<string, int>("123");
        DataConverter.Convert<int, string>(1);

        DataConverter.Convert<long, long>(1L);
        DataConverter.Convert<long, int>(1L);
        DataConverter.Convert<long, float>(1L);
        DataConverter.Convert<long, double>(1L);
        DataConverter.Convert<long, bool>(1L);
        DataConverter.Convert<string, long>("123358395839539");
        DataConverter.Convert<long, string>(1L);

        DataConverter.Convert<float, float>(1f);
        DataConverter.Convert<float, int>(1f);
        DataConverter.Convert<float, long>(1f);
        DataConverter.Convert<float, double>(1f);
        DataConverter.Convert<float, bool>(1f);
        DataConverter.Convert<string, float>("123.242857827857285782");
        DataConverter.Convert<float, string>(1f);

        DataConverter.Convert<double, float>(1d);
        DataConverter.Convert<double, int>(1d);
        DataConverter.Convert<double, long>(1d);
        DataConverter.Convert<double, double>(1d);
        DataConverter.Convert<double, bool>(1d);
        DataConverter.Convert<string, double>("123.242857827857285782");
        DataConverter.Convert<double, string>(1d);

        DataConverter.Convert<bool, int>(true);
        DataConverter.Convert<bool, long>(true);
        DataConverter.Convert<bool, float>(true);
        DataConverter.Convert<bool, double>(true);
        DataConverter.Convert<bool, bool>(true);
        DataConverter.Convert<string, bool>("True");
        DataConverter.Convert<bool, string>(true);

        DataConverter.Convert<ulong, int>(42);

        DataConverter.Convert<string, Color>(DataConverter.Convert<Color, string>(Color.red));
        DataConverter.Convert<Color32, string>(DataConverter.Convert<string, Color32>("#22FF3377"));
        DataConverter.Convert<string, Vector2>(DataConverter.Convert<Vector3, string>(new Vector3(3, 5, 1.4f)));

        DataConverter.Convert<Color, int>(Color.red);*/

        var json = @"
            {
                ""id"": 123,
                ""name"": ""reward"",
                ""items"":
                {
                    ""item1"":
                    {
                        ""id"": 1,
                        ""count"": 231
                    },
                    ""item2"":
                    {
                        ""id"": 2,
                        ""user"": null,
                        ""count"": 41
                    }
                },
                ""extra"":
                [
                    231.13,
                    2,
                    ""comment"": ""some_value"",
                    3.424,
                    4,
                    null,
                    {
                        ""enabled"": true
                    }
                ]
            }";

        var dataSource = DataSourceFactory.FromJson("root", json);

        //TraverseDataSource(dataSource);

        if (dataSource.TryGetNodeByPath<IDataProperty>("extra.6.enabled", out var property))
        {
            Debug.Log(property.GetValue<string>());
        }
    }

    static void TraverseDataSource(IDataSource dataSource)
    {
        dataSource.ForEach<IDataNode>(node =>
        {
            if (node is IDataSource nestedDataSource)
            {
                Debug.Log($"Nested dataSource '{nestedDataSource.Name}':");

                TraverseDataSource(nestedDataSource);
            }
            else if (node is IDataProperty dataProperty)
            {
                Debug.Log($"Property '{dataProperty.Name}' ({node.GetType().Name}) = {dataProperty.GetValue<string>()}");
            }
        });
    }
#endif
}
