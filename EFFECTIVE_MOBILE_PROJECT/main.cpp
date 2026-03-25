#include <iostream>
#include <fstream>
#include <vector>
#include <unordered_map>

struct ListNode 
{
    ListNode* prev = nullptr;
    ListNode* next = nullptr;
    ListNode* rand = nullptr;
    std::string data;
};

class ListManager
{
public:
    ListNode* parseInLetFile(const std::string& fileName)
    {
        std::ifstream file(fileName);
        if (!file.is_open())
        {
            std::cerr << "Failed to open file: " << fileName << std::endl;
            return nullptr;
        }

        std::vector<ListNode*> nodes;
        std::vector<int> randomIndexes;
        std::string line;

        while (std::getline(file, line))
        {
            size_t lastSemicolonPos = line.rfind(';');
            if (lastSemicolonPos == std::string::npos) 
            {
                std::cerr << "Invalid line format: " << line << std::endl;
                continue;
            }

            ListNode* node = new ListNode();
            node->data = line.substr(0, lastSemicolonPos);

            try 
            {
                int randIndex = std::stoi(line.substr(lastSemicolonPos + 1));
                randomIndexes.push_back(randIndex);
            }
            catch(...)
            {
                randomIndexes.push_back(-1);
            }

            if (!nodes.empty())
            {
                nodes.back()->next = node;
                node->prev = nodes.back();
            }
            nodes.push_back(node);
        }

        for (size_t i = 0; i < nodes.size(); i++)
        {
            if (randomIndexes[i] >= 0 && randomIndexes[i] < (int)nodes.size())
            {
                nodes[i]->rand = nodes[randomIndexes[i]];
            }
        }
        return nodes.empty() ? nullptr : nodes[0];
    }

    void serialize(ListNode* head, const std::string& filename)
    {
        std::ofstream file(filename, std::ios::binary);
        if (!file.is_open()) return;

        std::unordered_map<ListNode*, int> nodeToIndex;
        ListNode* current = head;
        int count = 0;

        while (current)
        {
            nodeToIndex[current] = count++;
            current = current->next;
        }

        file.write(reinterpret_cast<char*>(&count), sizeof(count));

        current = head;
        while (current)
        {
            uint32_t dataSize = static_cast<uint32_t>(current->data.size());
            file.write(reinterpret_cast<char*>(&dataSize), sizeof(dataSize));
            file.write(current->data.c_str(), dataSize);

            int randomIndex = current->rand ? nodeToIndex[current->rand] : -1;
            file.write(reinterpret_cast<char*>(&randomIndex), sizeof(randomIndex));
            current = current->next;
        }
    }

    ListNode* deserialize(const std::string& fileName)
    {
        std::ifstream file(fileName, std::ios::binary);
        if (!file.is_open())
        {
            std::cerr << "Failed to open file: " << fileName << std::endl;
            return nullptr;
        }

        int count = 0;
        file.read(reinterpret_cast<char*>(&count), sizeof(count));  

        if (count <= 0)
        {
            std::cerr << "Invalid node count: " << count << std::endl;
            return nullptr;
        }

        std::vector<ListNode*> nodes(count);
        std::vector<int> randomIndexes(count);

        for (int i = 0; i < count; i++)
        {
            nodes[i] = new ListNode();
            uint32_t dataSize;
            file.read(reinterpret_cast<char*>(&dataSize), sizeof(dataSize));    

            nodes[i]->data.resize(dataSize);
            file.read(&nodes[i]->data[0], dataSize);

            file.read(reinterpret_cast<char*>(&randomIndexes[i]), sizeof(randomIndexes[i]));

            if (i > 0)
            {
                nodes[i - 1]->next = nodes[i];
                nodes[i]->prev = nodes[i - 1];
            }
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = randomIndexes[i];
            if (randomIndex >= 0 && randomIndex < count)
            {
                nodes[i]->rand = nodes[randomIndexes[i]];
            }
            else 
            {
                nodes[i]->rand = nullptr;
            }
        }
        return nodes[0];
    }

    void saveAsText(ListNode* head, const std::string& filename)
    {
        std::ofstream file(filename);
        if (!file.is_open())
        {
            std::cerr << "Failed to open file: " << filename << std::endl;
            return;
        }

        std::unordered_map<ListNode*, int> nodeToIndex;
        ListNode* current = head;
        int index = 0;
        while (current) 
        {
            nodeToIndex[current] = index++;
            current = current->next;
        }

        current = head;
        while (current)
        {
            file << current->data << ";";
            if (current->rand && nodeToIndex.count(current->rand)) {
                file << nodeToIndex[current->rand];
            } 
            else 
            {
                file << -1;
            }
            file << "\n";
            current = current->next;
        }
    }

    void clear(ListNode* head)
    {
        while (head)
        {
            ListNode* temp = head;
            head = head->next;
            delete temp;
        }
    }
};

int main()
{
    ListManager manager;

    ListNode* head = manager.parseInLetFile("inlet.in");
    if (head)
    {
        manager.serialize(head, "outlet.out");
        std::cout << "Serialization completed successfully." << std::endl;
    }
    else
    {
        std::cerr << "Failed to parse the input file." << std::endl;
    }

    ListNode* restoredHead = manager.deserialize("outlet.out");
    if (restoredHead)
    {
        manager.saveAsText(restoredHead, "restored.txt");
        std::cout << "Deserialization and text saving completed successfully." << std::endl;
    }
    else
    {
        std::cerr << "Failed to deserialize the output file." << std::endl;
    }

    manager.clear(head);  
}