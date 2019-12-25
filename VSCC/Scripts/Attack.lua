Engine:RegisterScript("Attack Roll", 
	function(arg)
		if not Roll20:IsConnected() then
			Engine:ShowMessage("Not connected", "You are not connected to a Roll20 server. Connect first before using this script.");
		else
			local t = {};
			t["Weapon name"] = "Held Weapon";
			t["Weapon dice"] = "1d10";
			t["Weapon extra dice"] = "";
			t["Weapon extra damage"] = 0;
			t["Weapon extra chance to hit"] = 0;
			t["Weapon attacks with strength"] = true;
			t["Has profficiency"] = true;
			t["Is extra attack"] = false;
			if Engine:ShowContextWindow("A weapon attack roll. Weapon dice is the damage dice of your weapon. Make sure you increment it if you are dual-wielding.\nWeapon extra dice is the extra damage dice your weapon might or might not have. If you do not have it leave it empty.\nWeapon attacks with strength is checked if you attack with your strength stat, uncheck to use dexterity instead.\nIs extra attack dictates whether to add the attack stat to the weapon damage.", t) then
				-- Start setting up the roll string.
				local text = "&{template:default} {{name=Weapon Attack}} ";
				
				-- Add character name to attack
				text = text .. "{{" .. State["General"]["Name"] .. " attacks with " .. t["Weapon name"] .. "}} ";

				-- Damage and attack roll stat
				local modStat = State["General"]["StatModDex"];
				if t["Weapon attacks with strength"] then
					modStat = State["General"]["StatModStr"];
				end
				
				-- Add hit roll
				local attackRoll = "[[1d20+" .. tostring(modStat) .. "+" .. t["Weapon extra chance to hit"];
				if t["Has profficiency"] then
					attackRoll = attackRoll .. "+" .. State["General"]["ProfficiencyBonus"];
				end

				attackRoll = attackRoll .. "]]";
				text = text .. "{{attack=" .. attackRoll .. " | " .. attackRoll .. "VS AC}} ";

				-- Add damage roll
				local damageRoll = "[[" .. t["Weapon dice"];
				if not t["Is extra attack"] then
					damageRoll = damageRoll .. "+" .. modStat;
				end

				damageRoll = damageRoll .. "+" .. t["Weapon extra damage"];
				if not (t["Weapon extra dice"] == nil or t["Weapon extra dice"] == '') then
					damageRoll = damageRoll .. "+" .. t["Weapon extra dice"];
				end

				damageRoll = damageRoll .. "]]";
				text = text .. "{{damage=" .. damageRoll .. " Crit? +[[" .. t["Weapon dice"] .. "]]}}";
				Roll20:Message(text);
			end
		end
	end
);