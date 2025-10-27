using Bogus;

namespace CleanAspCore.TestUtils.Fakers;

public delegate Faker<T> FakerConfigurator<T>(Faker<T> t) where T : class;
