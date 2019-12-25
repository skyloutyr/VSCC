Engine:RegisterScript("Initiative Roll", 
	function(arg)
		if not Roll20:IsConnected() then
			Engine:ShowMessage("Not connected", "You are not connected to a Roll20 server. Connect first before using this script.");
		else
			local t = {};
			t["Modifier"] = 0;
			if Engine:ShowContextWindow("Enter your initiative roll modifier (if any)", t) then
				local modExtra = t["Modifier"];
				local modStats = State["General"]["StatModDex"];
				local text = "&{template:default} {{name=" .. State["General"]["Name"] .. ": Initiative}} {{result=[[1d20+" .. tostring(modExtra + modStats) .. " &{tracker}]]}}";
				Roll20:Message(text);
			end
		end
	end);