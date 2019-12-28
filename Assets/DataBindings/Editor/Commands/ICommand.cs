namespace Realmar.DataBindings.Editor.Commands
{
	// TODO reevaluate if command is the right approach, I put lots of hacks in there with mediators in order to pass data
	// between commands. Also command creation and execution is tightly coupled. (order of execution AND data) It will
	// produce wrong results if commands are executed twice. I get no benefit in decoupling the initiator from the caller.
	// In fact I immediately call execute after constructing the graph, so whats the point? I could drawing some diagram
	// on how the binding is weaved. But does that really justify the complexity of command + composite + mediator?
	// Decoupling graph construction from command execution makes debugging hard, because it is not obvious why
	// a certain command is executed while being in the debugger. Having mediators passing data between commands
	// obfuscates things even more because not only do you need to figure out why this command was called but also
	// what the origin of the dat is. There are multiple data sources and commands are reused, so
	// finding out about these things require figuring out the context and then manually checking how the graph is
	// created and then start debugging the location where the command is created.
	//
	// Having simple method calls which immediately weave/emit would be clearer and would remove the questionable
	// decoupling of initiator and caller.
	//
	// What about abstraction? Can we just use static classes and methods?
	internal interface ICommand
	{
		void Execute();
	}
}
