// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using NSubstitute;

namespace Charites.Windows.Mvc;

[Specification("ContentNavigator Spec")]
class ContentNavigatorSpec : FixtureSteppable
{
    IContentNavigator Navigator { get; } = new ContentNavigator();

    EventHandler NavigationStateChangedHandler { get; } = Substitute.For<EventHandler>();
    EventHandler<ContentNavigatingEventArgs> NavigatingHandler { get; } = Substitute.For<EventHandler<ContentNavigatingEventArgs>>();
    EventHandler<ContentNavigatedEventArgs> NavigatedHandler { get; } = Substitute.For<EventHandler<ContentNavigatedEventArgs>>();

    NavigationContents.FirstContent FirstContent { get; } = new();
    NavigationContents.SecondContent SecondContent { get; } = new();
    NavigationContents.ThirdContent ThirdContent { get; } = new();

    bool NavigationResult { get; set; }

    public ContentNavigatorSpec()
    {
        Navigator.NavigationStateChanged += NavigationStateChangedHandler;
        Navigator.Navigating += NavigatingHandler;
        Navigator.Navigated += NavigatedHandler;
    }

    void ClearEventHandlerReceivedCalls()
    {
        NavigationStateChangedHandler.ClearReceivedCalls();
        NavigatingHandler.ClearReceivedCalls();
        NavigatedHandler.ClearReceivedCalls();
    }

    [Example("Initial states")]
    void Ex01()
    {
        Expect("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Expect("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Expect("the navigation stack should be enabled", () => Navigator.IsNavigationStackEnabled);

        Expect("the current content should be the InitialContent", () => Navigator.CurrentContent == ContentNavigator.InitialContent);
        Expect("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Expect("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("Navigates to the specified content")]
    void Ex02()
    {
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the first content", () => Navigator.CurrentContent == FirstContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the second content", () => Navigator.CurrentContent == SecondContent);
        Then("the backward navigation history should be stacked", () => Navigator.BackwardStack.SequenceEqual(new[] { FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should be stacked", () => Navigator.BackwardStack.SequenceEqual(new object[] { SecondContent, FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should not be changed", () => Navigator.BackwardStack.SequenceEqual(new object[] { SecondContent, FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the InitialContent", () => NavigationResult = Navigator.NavigateTo(ContentNavigator.InitialContent));
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should not be changed", () => Navigator.BackwardStack.SequenceEqual(new object[] { SecondContent, FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("Goes the backward/forward navigation history")]
    void Ex03()
    {
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        When("to go the backward navigation history", () => NavigationResult = Navigator.GoBackward());
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should be able to go forward", () => Navigator.CanGoForward);
        Then("the current content should be the second content", () => Navigator.CurrentContent == SecondContent);
        Then("the backward navigation history should be popped", () => Navigator.BackwardStack.SequenceEqual(new[] { FirstContent }));
        Then("the forward navigation history should be stacked", () => Navigator.ForwardStack.SequenceEqual(new[] { ThirdContent }));

        When("to go the backward navigation history", () => NavigationResult = Navigator.GoBackward());
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should be able to go forward", () => Navigator.CanGoForward);
        Then("the current content should be the first content", () => Navigator.CurrentContent == FirstContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be stacked", () => Navigator.ForwardStack.SequenceEqual(new object[] { SecondContent, ThirdContent }));

        When("to go the backward navigation history", () => NavigationResult = Navigator.GoBackward());
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should be able to go forward", () => Navigator.CanGoForward);
        Then("the current content should be the first content", () => Navigator.CurrentContent == FirstContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should not be changed", () => Navigator.ForwardStack.SequenceEqual(new object[] { SecondContent, ThirdContent }));

        When("to go the forward navigation history", () => NavigationResult = Navigator.GoForward());
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should be able to go forward", () => Navigator.CanGoForward);
        Then("the current content should be the second content", () => Navigator.CurrentContent == SecondContent);
        Then("the backward navigation history should be stacked", () => Navigator.BackwardStack.SequenceEqual(new[] { FirstContent }));
        Then("the forward navigation history should be popped", () => Navigator.ForwardStack.SequenceEqual(new[] { ThirdContent }));

        When("to go the forward navigation history", () => NavigationResult = Navigator.GoForward());
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should be stacked", () => Navigator.BackwardStack.SequenceEqual(new object[] { SecondContent, FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to go the backward navigation history", () => NavigationResult = Navigator.GoBackward());
        When("to go the backward navigation history", () => NavigationResult = Navigator.GoBackward());
        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the second content", () => Navigator.CurrentContent == SecondContent);
        Then("the backward navigation history should be stacked", () => Navigator.BackwardStack.SequenceEqual(new[] { FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("When the navigation stack is disabled")]
    void Ex04()
    {
        Given("the navigation stack is disabled", () => Navigator.IsNavigationStackEnabled = false);
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the first content", () => Navigator.CurrentContent == FirstContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the second content", () => Navigator.CurrentContent == SecondContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());

        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        Then("the navigation should be success", () => NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("Cancels the new navigation")]
    void Ex05()
    {
        When("to set the Navigating event handler in order to cancel the navigation", () => Navigator.Navigating += (_, e) => e.Cancel = true);
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the InitialContent", () => Navigator.CurrentContent == ContentNavigator.InitialContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("Cancels the backward navigation")]
    void Ex06()
    {
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        When("to set the Navigating event handler in order to cancel the navigation", () => Navigator.Navigating += (_, e) => e.Cancel = true);
        When("to go the forward navigation history", () => NavigationResult = Navigator.GoBackward());
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should be able to go backward", () => Navigator.CanGoBackward);
        Then("the navigator should not be able to go forward", () => !Navigator.CanGoForward);
        Then("the current content should be the third content", () => Navigator.CurrentContent == ThirdContent);
        Then("the backward navigation history should not be changed", () => Navigator.BackwardStack.SequenceEqual(new object[] { SecondContent, FirstContent }));
        Then("the forward navigation history should be empty", () => !Navigator.ForwardStack.Any());
    }

    [Example("Cancels the forward navigation")]
    void Ex07()
    {
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        When("to navigate to the third content", () => NavigationResult = Navigator.NavigateTo(ThirdContent));
        When("to go the forward navigation history", () => NavigationResult = Navigator.GoBackward());
        When("to go the forward navigation history", () => NavigationResult = Navigator.GoBackward());
        When("to set the Navigating event handler in order to cancel the navigation", () => Navigator.Navigating += (_, e) => e.Cancel = true);
        When("to go the forward navigation history", () => NavigationResult = Navigator.GoForward());
        Then("the navigation should be failure", () => !NavigationResult);
        Then("the navigator should not be able to go backward", () => !Navigator.CanGoBackward);
        Then("the navigator should be able to go forward", () => Navigator.CanGoForward);
        Then("the current content should be the first content", () => Navigator.CurrentContent == FirstContent);
        Then("the backward navigation history should be empty", () => !Navigator.BackwardStack.Any());
        Then("the forward navigation history should not be changed", () => Navigator.ForwardStack.SequenceEqual(new object[] { SecondContent, ThirdContent }));
    }

    [Example("Handles the navigator events")]
    void Ex08()
    {
        When("to navigate to the first content", () => NavigationResult = Navigator.NavigateTo(FirstContent));
        Then("Navigating event handler should be called", () =>
            NavigatingHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatingEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.New && e.Content == FirstContent && e.SourceContent == ContentNavigator.InitialContent
            ))
        );
        Then("NavigationStateChanged event handler should be called", () => NavigationStateChangedHandler.Received(1).Invoke(Navigator, EventArgs.Empty));
        Then("Navigated event handler should be called", () =>
            NavigatedHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatedEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.New && e.Content == FirstContent && e.SourceContent == ContentNavigator.InitialContent
            ))
        );
        ClearEventHandlerReceivedCalls();

        When("to navigate to the second content", () => NavigationResult = Navigator.NavigateTo(SecondContent));
        Then("Navigating event handler should be called", () =>
            NavigatingHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatingEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.New && e.Content == SecondContent && e.SourceContent == FirstContent
            ))
        );
        Then("NavigationStateChanged event handler should be called", () => NavigationStateChangedHandler.Received(1).Invoke(Navigator, EventArgs.Empty));
        Then("Navigated event handler should be called", () =>
            NavigatedHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatedEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.New && e.Content == SecondContent && e.SourceContent == FirstContent
            ))
        );
        ClearEventHandlerReceivedCalls();

        When("to go to the backward history", () => NavigationResult = Navigator.GoBackward());
        Then("Navigating event handler should be called", () =>
            NavigatingHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatingEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.Backward && e.Content == FirstContent && e.SourceContent == SecondContent
            ))
        );
        Then("NavigationStateChanged event handler should be called", () => NavigationStateChangedHandler.Received(1).Invoke(Navigator, EventArgs.Empty));
        Then("Navigated event handler should be called", () =>
            NavigatedHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatedEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.Backward && e.Content == FirstContent && e.SourceContent == SecondContent
            ))
        );
        ClearEventHandlerReceivedCalls();

        When("to go to the forward history", () => NavigationResult = Navigator.GoForward());
        Then("Navigating event handler should be called", () =>
            NavigatingHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatingEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.Forward && e.Content == SecondContent && e.SourceContent == FirstContent
            ))
        );
        Then("NavigationStateChanged event handler should be called", () => NavigationStateChangedHandler.Received(1).Invoke(Navigator, EventArgs.Empty));
        Then("Navigated event handler should be called", () =>
            NavigatedHandler.Received(1).Invoke(Navigator, Arg.Is<ContentNavigatedEventArgs>(e =>
                e.NavigationMode == ContentNavigationMode.Forward && e.Content == SecondContent && e.SourceContent == FirstContent
            ))
        );
    }
}